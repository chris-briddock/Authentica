# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 7256 

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TestClient.csproj", "./"]
RUN dotnet restore "./TestClient.csproj"
COPY . .
RUN dotnet build "./TestClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TestClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY cert.pfx /app/cert.pfx
RUN chmod 644 /app/cert.pfx 
ENV ASPNETCORE_URLS=https://+:7256
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=YourSecurePassword
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/cert.pfx
USER app
ENTRYPOINT ["dotnet", "TestClient.dll"]