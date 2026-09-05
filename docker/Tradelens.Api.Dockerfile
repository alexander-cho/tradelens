FROM node:22-alpine AS angular-builder
WORKDIR /app

RUN npm install -g @angular/cli@20

COPY ["apps/web/package*.json", "./apps/web/"]
RUN cd apps/web && npm ci

COPY ["packages/", "./packages/"]
COPY ["apps/web/", "./apps/web/"]
# output angular app build to new directory, before it was being copied to nowhere, Api ends up serving stale wwwroot
RUN cd apps/web && ng build --configuration production --output-path /app/dist


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Hosts/Tradelens.Api/Tradelens.Api.csproj", "Hosts/Tradelens.Api/"]
COPY ["src/_legacy/Tradelens.Domain/Tradelens.Domain.csproj", "_legacy/Tradelens.Domain/"]
COPY ["src/_legacy/Tradelens.Infrastructure/Tradelens.Infrastructure.csproj", "_legacy/Tradelens.Infrastructure/"]
RUN dotnet restore "Hosts/Tradelens.Api/Tradelens.Api.csproj"

# https://devops.stackexchange.com/questions/17647/azure-devops-pipeline-failure-program-does-not-contain-a-static-main-method
# this line used to be after the below COPY commands!
WORKDIR /src/Hosts/Tradelens.Api

COPY ["src/Hosts/Tradelens.Api", "./"]
COPY ["src/_legacy/Tradelens.Domain", "./"]
COPY ["src/_legacy/Tradelens.Infrastructure", "./"]

COPY --from=angular-builder ["/app/dist/browser", "./wwwroot/"]

RUN dotnet publish "Tradelens.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
USER app
EXPOSE 6501

COPY --from=base ["/app/publish", "."]
ENTRYPOINT ["dotnet", "Tradelens.Api.dll"]