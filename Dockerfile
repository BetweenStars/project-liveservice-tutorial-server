# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore GameServer.csproj
RUN dotnet publish GameServer.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Render가 사용하는 포트
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "GameServer.dll"]