FROM node:22-alpine AS angular-builder
WORKDIR /app

RUN npm install -g @angular/cli@20

COPY ["apps/client-ng/package*.json", "./ClientApp/"]
RUN cd ClientApp && npm ci

COPY ["apps/client-ng/", "./ClientApp/"]
COPY ["apps/tradelens/src/Tradelens.Api/wwwroot", "./tradelens/src/Tradelens.Api/wwwroot/"]

RUN cd ClientApp && ng build --configuration production


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["apps/tradelens/src/Tradelens.Api/Tradelens.Api.csproj", "Tradelens.Api/"]
COPY ["apps/tradelens/src/Tradelens.Core/Tradelens.Core.csproj", "Tradelens.Core/"]
COPY ["apps/tradelens/src/Tradelens.Infrastructure/Tradelens.Infrastructure.csproj", "Tradelens.Infrastructure/"]
RUN dotnet restore "Tradelens.Api/Tradelens.Api.csproj"

COPY ["apps/tradelens/src/", "./"]

COPY --from=angular-builder ["/app/tradelens/src/Tradelens.Api/wwwroot", "./Tradelens.Api/wwwroot/"]

WORKDIR /src/Tradelens.Api
RUN dotnet publish "Tradelens.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
USER app
EXPOSE 6501

COPY --from=base ["/app/publish", "."]
ENTRYPOINT ["dotnet", "Tradelens.Api.dll"]