FROM node:22-alpine AS angular-builder
WORKDIR /app

RUN npm install -g @angular/cli@20

COPY ["apps/client-ng/package*.json", "./ClientApp/"]
RUN cd ClientApp && npm ci

COPY ["apps/client-ng/", "./ClientApp/"]
COPY ["apps/backend/src/API/wwwroot", "./backend/src/API/wwwroot/"]

RUN cd ClientApp && ng build --configuration production


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["apps/backend/src/API/API.csproj", "API/"]
COPY ["apps/backend/src/Core/Core.csproj", "Core/"]
COPY ["apps/backend/src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "API/API.csproj"

COPY ["apps/backend/src/", "./"]

COPY --from=angular-builder ["/app/backend/src/API/wwwroot", "./API/wwwroot/"]

WORKDIR /src/API
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
USER app
EXPOSE 6501

COPY --from=base ["/app/publish", "."]
ENTRYPOINT ["dotnet", "API.dll"]