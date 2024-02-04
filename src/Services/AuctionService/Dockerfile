FROM mcr.microsoft.com/dotnet/aspnet:7.0 as base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS publish
COPY MicroservicesTestProject.sln MicroservicesTestProject.sln
COPY /src/Core/Core.Extensions/Core.Extensions.csproj /src/Core/Core.Extensions/Core.Extensions.csproj
COPY /src/Services/AuctionService/AuctionService.csproj /src/Services/AuctionService/AuctionService.csproj
COPY /src/Services/GatewayService/GatewayService.csproj /src/Services/GatewayService/GatewayService.csproj
COPY /src/Services/SearchService/SearchService.csproj /src/Services/SearchService/SearchService.csproj
COPY /src/Services/BidService/BidService.csproj /src/Services/BidService/BidService.csproj
COPY /src/Services/NotificationService/NotificationService.csproj /src/Services/NotificationService/NotificationService.csproj
RUN dotnet restore 

COPY /src/Services/AuctionService /src/Services/AuctionService
WORKDIR /src/Services/AuctionService
RUN dotnet publish -c Release -o /app

FROM base AS FINAL
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AuctionService.dll"]

