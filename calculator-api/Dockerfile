FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY shared-contracts/shared-contracts.csproj ./shared-contracts/
COPY calculator-api/calculator-api.csproj ./calculator-api/

WORKDIR /src/calculator-api
RUN dotnet restore

WORKDIR /src
COPY calculator-api/. ./calculator-api/
COPY shared-contracts/. ./shared-contracts/

WORKDIR /src/calculator-api
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "calculator-api.dll"]