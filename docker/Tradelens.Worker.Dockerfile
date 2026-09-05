FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Hosts/Tradelens.Worker/Tradelens.Worker.csproj", "Hosts/Tradelens.Worker/"]
COPY ["src/_legacy/Tradelens.Domain/Tradelens.Domain.csproj", "_legacy/Tradelens.Domain/"]
COPY ["src/_legacy/Tradelens.Infrastructure/Tradelens.Infrastructure.csproj", "_legacy/Tradelens.Infrastructure/"]
RUN dotnet restore "Hosts/Tradelens.Worker/Tradelens.Worker.csproj"

# https://devops.stackexchange.com/questions/17647/azure-devops-pipeline-failure-program-does-not-contain-a-static-main-method
# this line used to be after the below COPY commands!
WORKDIR /src/Hosts/Tradelens.Worker

COPY ["src/Hosts/Tradelens.Worker", "./"]
COPY ["src/_legacy/Tradelens.Domain", "./"]
COPY ["src/_legacy/Tradelens.Infrastructure", "./"]

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tradelens.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
USER app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tradelens.Worker.dll"]
