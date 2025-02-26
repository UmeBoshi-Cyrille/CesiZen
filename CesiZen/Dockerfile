#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
RUN mkdir /app/Logs

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CesiZen/CesiZen.Api.csproj", "CesiZen/"]
COPY ["CesiZen.Application/CesiZen.Application.csproj", "CesiZen.Application/"]
COPY ["CesiZen.Domain/CesiZen.Domain.csproj", "CesiZen.Domain/"]
COPY ["CesiZen.Infrastructure/CesiZen.Infrastructure.csproj", "CesiZen.Infrastructure/"]
RUN dotnet restore "./CesiZen/CesiZen.Api.csproj"
COPY . .
WORKDIR "/src/CesiZen"
RUN dotnet build "./CesiZen.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CesiZen.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CesiZen.Api.dll"]