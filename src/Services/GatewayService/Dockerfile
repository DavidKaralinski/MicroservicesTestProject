FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY MicroservicesTestProject.sln MicroservicesTestProject.sln
COPY /src/Core/Core.Extensions/Core.Extensions.csproj /src/Core/Core.Extensions/Core.Extensions.csproj
COPY /src/Services/AuctionService/AuctionService.csproj /src/Services/AuctionService/AuctionService.csproj
COPY /src/Services/IdentityService/IdentityService.csproj /src/Services/IdentityService/IdentityService.csproj
COPY /src/Services/GatewayService/GatewayService.csproj /src/Services/GatewayService/GatewayService.csproj
COPY /src/Services/SearchService/SearchService.csproj /src/Services/SearchService/SearchService.csproj
COPY /src/Services/IdentityService/IdentityService.csproj /src/Services/IdentityService/IdentityService.csproj
RUN dotnet restore 

COPY /src/Services/GatewayService /src/Services/GatewayService
WORKDIR /src/Services/GatewayService
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "GatewayService.dll"]
