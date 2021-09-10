FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /FileForwaderCore
ARG Version=0.0.1.0
COPY . .
RUN dotnet build FileForwaderCore.csproj -c Debug -o /app/build -p:Version=$Version
RUN dotnet publish FileForwaderCore.csproj -c Debug -o /app/publish -p:Version=$Version

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FileForwaderCore.dll"]
EXPOSE 80
EXPOSE 443
