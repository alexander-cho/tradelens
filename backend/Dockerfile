FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 6501

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/API.csproj", "API/"]
COPY ["src/Core/Core.csproj", "Core/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "API/API.csproj"

COPY src .
WORKDIR /src/API
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

COPY src/Infrastructure/Data/SeedData /app/Infrastructure/Data/SeedData

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /app/Infrastructure/Data/SeedData /app/Infrastructure/Data/SeedData
USER app
ENTRYPOINT ["dotnet", "API.dll"]