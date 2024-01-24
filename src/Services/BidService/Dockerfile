FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY MicroservicesTestProject.sln MicroservicesTestProject.sln
COPY /src/Core/Core.Extensions/Core.Extensions.csproj /src/Core/Core.Extensions/Core.Extensions.csproj
COPY /src/Services/AuctionService/AuctionService.csproj /src/Services/AuctionService/AuctionService.csproj
COPY /src/Services/IdentityService/IdentityService.csproj /src/Services/IdentityService/IdentityService.csproj
COPY /src/Services/GatewayService/GatewayService.csproj /src/Services/GatewayService/GatewayService.csproj
COPY /src/Services/SearchService/SearchService.csproj /src/Services/SearchService/SearchService.csproj
COPY /src/Services/BidService/BidService.csproj /src/Services/BidService/BidService.csproj
RUN dotnet restore

COPY /src/Services/BidService /src/Services/BidService
WORKDIR /src/Services/BidService
RUN dotnet publish -c Release -o /app 

FROM base AS final
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "BidService.dll" ]