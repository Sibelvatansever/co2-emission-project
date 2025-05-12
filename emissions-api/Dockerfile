FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["emissions-api/emissions-api.csproj", "./emissions-api/"]
RUN dotnet restore "./emissions-api/emissions-api.csproj"

COPY . .
WORKDIR "/src/emissions-api"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "emissions-api.dll"]