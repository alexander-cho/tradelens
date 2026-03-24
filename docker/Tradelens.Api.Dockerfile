FROM node:22-alpine AS angular-builder
WORKDIR /app

RUN npm install -g @angular/cli@20

COPY ["apps/client-ng/package*.json", "./apps/client-ng/"]
RUN cd apps/client-ng && npm ci

COPY ["packages/", "./packages/"]
COPY ["apps/client-ng/", "./apps/client-ng/"]
# output angular app build to new directory, before it was being copied to nowhere, Api ends up serving stale wwwroot
RUN cd apps/client-ng && ng build --configuration production --output-path /app/dist


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["apps/tradelens/src/Tradelens.Api/Tradelens.Api.csproj", "Tradelens.Api/"]
COPY ["apps/tradelens/src/Tradelens.Core/Tradelens.Core.csproj", "Tradelens.Core/"]
COPY ["apps/tradelens/src/Tradelens.Infrastructure/Tradelens.Infrastructure.csproj", "Tradelens.Infrastructure/"]
RUN dotnet restore "Tradelens.Api/Tradelens.Api.csproj"

# https://devops.stackexchange.com/questions/17647/azure-devops-pipeline-failure-program-does-not-contain-a-static-main-method
# this line used to be after the below COPY commands!
WORKDIR /src/Tradelens.Api

COPY ["apps/tradelens/src/Tradelens.Api", "./"]
COPY ["apps/tradelens/src/Tradelens.Core", "./"]
COPY ["apps/tradelens/src/Tradelens.Infrastructure", "./"]

COPY --from=angular-builder ["/app/dist/browser", "./wwwroot/"]

RUN dotnet publish "Tradelens.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
USER app
EXPOSE 6501

COPY --from=base ["/app/publish", "."]
ENTRYPOINT ["dotnet", "Tradelens.Api.dll"]