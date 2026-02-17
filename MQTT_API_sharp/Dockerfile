FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8055
EXPOSE 8056

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MQTT_API_sharp/MQTT_API_sharp.csproj", "MQTT_API_sharp/"]
RUN dotnet restore "MQTT_API_sharp/MQTT_API_sharp.csproj"
COPY . .
WORKDIR "/src/MQTT_API_sharp"
RUN dotnet build "./MQTT_API_sharp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MQTT_API_sharp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MQTT_API_sharp.dll"]

#docker run -d \
#  -p 5103:8080 \
#  -e "RepositorySettings__ConnectionString=Host=твой_хост;Database=твой_db;Username=логин;Password=СЕКРЕТНЫЙ_ПАРОЛЬ" \
#  tvoy_login/mqtt-api-sharp:v1
