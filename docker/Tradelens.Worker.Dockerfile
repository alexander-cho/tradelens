FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["tradelens/src/Tradelens.Worker/Tradelens.Worker.csproj", "Tradelens.Worker/"]
COPY ["tradelens/src/Tradelens.Domain/Tradelens.Domain.csproj", "Tradelens.Domain/"]
COPY ["tradelens/src/Tradelens.Infrastructure/Tradelens.Infrastructure.csproj", "Tradelens.Infrastructure/"]
RUN dotnet restore "Tradelens.Worker/Tradelens.Worker.csproj"

# https://devops.stackexchange.com/questions/17647/azure-devops-pipeline-failure-program-does-not-contain-a-static-main-method
# this line used to be after the below COPY commands!
WORKDIR /src/Tradelens.Worker

COPY ["tradelens/src/Tradelens.Worker", "./"]
COPY ["tradelens/src/Tradelens.Domain", "./"]
COPY ["tradelens/src/Tradelens.Infrastructure", "./"]

RUN dotnet build "Tradelens.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tradelens.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
USER app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tradelens.Worker.dll"]
