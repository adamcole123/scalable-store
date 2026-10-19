
# Build the .NET 10 application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY . .

RUN dotnet restore "Store.BasketService/Store.BasketService/Store.BasketService.csproj"

RUN dotnet publish "Store.BasketService/Store.BasketService/Store.BasketService.csproj" \
    -c Release \
    --no-restore \
    -o /app/publish

# Small runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Store.BasketService.dll"]
