# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 8080

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# COPY . .
# RUN dotnet restore "MyApp.csproj"
# RUN dotnet build "MyApp.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "MyApp.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "MyApp.dll"]
# ──────────────────────────────────────────────────────────────
# Stage 1 – Base runtime
# ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# ──────────────────────────────────────────────────────────────
# Stage 2 – Restore (separate stage for caching)
# ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /src

# Copy just project files
COPY *.sln .
COPY MyApp/MyApp.csproj MyApp/
COPY MyApp.Tests/MyApp.Tests.csproj MyApp.Tests/

# Restore dependencies
RUN dotnet restore

# ──────────────────────────────────────────────────────────────
# Stage 3 – Build
# ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy restored dependencies from restore stage
COPY --from=restore /src /src
COPY --from=restore /root/.nuget /root/.nuget

# Copy remaining source code
COPY MyApp/. MyApp/
COPY MyApp.Tests/. MyApp.Tests/

# Build and publish
WORKDIR /src/MyApp
RUN dotnet publish MyApp.csproj -c Release -o /app/publish

# ──────────────────────────────────────────────────────────────
# Stage 4 – Final runtime image
# ──────────────────────────────────────────────────────────────
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyApp.dll"]