FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY MicroservicesTestProject.sln MicroservicesTestProject.sln
COPY ./src/Core/Core.Extensions/Core.Extensions.csproj /src/Core/Core.Extensions/Core.Extensions.csproj
COPY ./src/Services/AuctionService/AuctionService.csproj /src/Services/AuctionService/AuctionService.csproj
COPY ./src/Services/GatewayService/GatewayService.csproj /src/Services/GatewayService/GatewayService.csproj
COPY ./src/Services/SearchService/SearchService.csproj /src/Services/SearchService/SearchService.csproj
COPY ./src/Services/BidService/BidService.csproj /src/Services/BidService/BidService.csproj
COPY ./src/Services/NotificationService/NotificationService.csproj /src/Services/NotificationService/NotificationService.csproj
RUN dotnet restore 

COPY ./src/Services/SearchService /src/Services/SearchService
WORKDIR /src/Services/SearchService
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "SearchService.dll"]
