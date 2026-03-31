# Dockerfile za BHASCoreProject - .NET 10
# Multi-stage build za optimizaciju veličine image-a

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
# HTTPS port onemogućen za development u Docker-u

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopiraj project fajlove i restore dependencies
COPY ["BHASCoreProject/BHASCore.Web.csproj", "BHASCoreProject/"]
COPY ["BHASCore.Data.Business/BHASCore.Data.Business.csproj", "BHASCore.Data.Business/"]
COPY ["BHASCore.Data.Identity/BHASCore.Data.Identity.csproj", "BHASCore.Data.Identity/"]

RUN dotnet restore "BHASCoreProject/BHASCore.Web.csproj"

# Kopiraj sve fajlove i build
COPY . .
WORKDIR "/src/BHASCoreProject"
RUN dotnet build "BHASCore.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BHASCore.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BHASCore.Web.dll"]
