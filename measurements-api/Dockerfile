FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["measurements-api/measurements-api.csproj", "./measurements-api/"]
RUN dotnet restore "./measurements-api/measurements-api.csproj"

COPY . .
WORKDIR "/src/measurements-api"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "measurements-api.dll"]