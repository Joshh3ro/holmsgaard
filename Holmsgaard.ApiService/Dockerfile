FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Holmsgaard.ApiService/Holmsgaard.ApiService.csproj", "Holmsgaard.ApiService/"]
COPY ["Holmsgaard.ServiceDefaults/Holmsgaard.ServiceDefaults.csproj", "Holmsgaard.ServiceDefaults/"]
RUN dotnet restore "Holmsgaard.ApiService/Holmsgaard.ApiService.csproj"

COPY . .
WORKDIR /src/Holmsgaard.ApiService
RUN dotnet publish "Holmsgaard.ApiService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
EXPOSE 8080
HEALTHCHECK --interval=10s --timeout=3s --retries=10 CMD curl --fail http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "Holmsgaard.ApiService.dll"]
