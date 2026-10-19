param location string = resourceGroup().location
param productImage string
param basketImage string

@secure()
param sqlAdminPassword string

var suffix = uniqueString(resourceGroup().id)


resource apiGateway 'Microsoft.App/managedEnvironments/httpRouteConfigs@2026-01-01' = {
  parent: containerEnv
  name: 'shopapi'

  properties: {
    rules: [
      {
        description: 'Route product requests'

        routes: [
          {
            match: {
              pathSeparatedPrefix: '/api/products'
            }
          }
        ]

        targets: [
          {
            containerApp: productService.name
          }
        ]
      }
      {
        description: 'Route basket requests'

        routes: [
          {
            match: {
              pathSeparatedPrefix: '/api/basket'
            }
          }
        ]

        targets: [
          {
            containerApp: basketService.name
          }
        ]
      }
    ]
  }

  dependsOn: [
    productService
    basketService
  ]
}

output apiGatewayUrl string = 'https://${apiGateway.properties.fqdn}'


// Angular hosting
resource frontend 'Microsoft.Web/staticSites@2025-03-01' = {
  name: 'shop-ui-${suffix}'
  location: location
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {}
}

// Shared environment for both microservices
resource containerEnv 'Microsoft.App/managedEnvironments@2026-01-01' = {
  name: 'shop-env-${suffix}'
  location: location
  properties: {}
}

resource sqlServer 'Microsoft.Sql/servers@2023-08-01' = {
  name: 'shop-sql-${suffix}'
  location: location
  properties: {
    administratorLogin: 'shopadmin'
    administratorLoginPassword: sqlAdminPassword
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
  }
}

resource productsDb 'Microsoft.Sql/servers/databases@2023-08-01' = {
  parent: sqlServer
  name: 'Products'
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}

// Learning environment only.
// Allows connections from Azure services broadly.
resource devSqlFirewall 'Microsoft.Sql/servers/firewallRules@2023-08-01' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource redis 'Microsoft.Cache/redisEnterprise@2025-04-01' = {
  name: 'shop-redis-${suffix}'
  location: location
  sku: {
    name: 'Balanced_B0'
  }
  properties: {
    encryption: {}
    highAvailability: 'Enabled'
    minimumTlsVersion: '1.2'
  }
}

resource redisDb 'Microsoft.Cache/redisEnterprise/databases@2025-04-01' = {
  parent: redis
  name: 'default'
  properties: {
    clientProtocol: 'Encrypted'
    accessKeysAuthentication: 'Enabled'
    clusteringPolicy: 'OSSCluster'
    evictionPolicy: 'VolatileLRU'
    port: 10000
  }
}

var sqlConnection = 'Server=tcp:${sqlServer.name}.database.windows.net,1433;Database=Products;User ID=shopadmin;Password=${sqlAdminPassword};Encrypt=True;TrustServerCertificate=False'

resource productService 'Microsoft.App/containerApps@2026-01-01' = {
  name: 'product-service'
  location: location
  properties: {
    environmentId: containerEnv.id

    configuration: {
      ingress: {
        external: false
        targetPort: 8080
        transport: 'auto'
        corsPolicy: {
          allowedOrigins: [
            'https://${frontend.properties.defaultHostname}'
            'http://localhost:4200'
          ]
          allowedMethods: ['GET', 'POST', 'PUT', 'DELETE']
          allowedHeaders: ['*']
        }
      }
      secrets: [
        {
          name: 'sql-connection'
          value: sqlConnection
        }
      ]
    }

    template: {
      containers: [
        {
          name: 'product-api'
          image: productImage
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
          env: [
            {
              name: 'ConnectionStrings__Products'
              secretRef: 'sql-connection'
            }
          ]
        }
      ]

      scale: {
        minReplicas: 1
        maxReplicas: 10
        rules: [
          {
            name: 'http-scaling'
            http: {
              metadata: {
                concurrentRequests: '50'
              }
            }
          }
        ]
      }
    }
  }
  dependsOn: [productsDb, devSqlFirewall]
}

var redisConnection = '${redis.properties.hostName}:10000,password=${redisDb.listKeys().primaryKey},ssl=True,abortConnect=False'

resource basketService 'Microsoft.App/containerApps@2026-01-01' = {
  name: 'basket-service'
  location: location
  properties: {
    environmentId: containerEnv.id

    configuration: {
      ingress: {
        external: false
        targetPort: 8080
        transport: 'auto'
        corsPolicy: {
          allowedOrigins: [
            'https://${frontend.properties.defaultHostname}'
            'http://localhost:4200'
          ]
          allowedMethods: ['GET', 'POST', 'PUT', 'DELETE']
          allowedHeaders: ['*']
        }
      }
      secrets: [
        {
          name: 'redis-connection'
          value: redisConnection
        }
      ]
    }

    template: {
      containers: [
        {
          name: 'basket-api'
          image: basketImage
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
          env: [
            {
              name: 'Redis__ConnectionString'
              secretRef: 'redis-connection'
            }
          ]
        }
      ]

      scale: {
        minReplicas: 1
        maxReplicas: 20
        rules: [
          {
            name: 'http-scaling'
            http: {
              metadata: {
                concurrentRequests: '50'
              }
            }
          }
        ]
      }
    }
  }
  dependsOn: [redisDb]
}

output frontendUrl string = 'https://${frontend.properties.defaultHostname}'

output productApiUrl string = 'https://${productService.properties.configuration.ingress.fqdn}'

output basketApiUrl string = 'https://${basketService.properties.configuration.ingress.fqdn}'
