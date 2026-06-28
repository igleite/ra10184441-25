# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Api.sln ./
COPY src/ ./src/

RUN dotnet restore Api.sln

RUN dotnet publish src/Modules/Database/DatabaseMigrator/DatabaseMigrator.csproj \
    -c Release \
    -o /app/migrator \
    --no-restore

RUN dotnet publish src/Api/Api.csproj \
    -c Release \
    -o /app/api \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api
WORKDIR /app

COPY --from=build /app/api ./

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Api.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS migrator
WORKDIR /app

COPY --from=build /app/migrator ./

ENTRYPOINT ["dotnet", "DatabaseMigrator.dll"]
