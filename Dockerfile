# 1. Usar la imagen oficial de Microsoft para compilar el código
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 2. Copiar los archivos y restaurar dependencias
COPY ["EvaluacionProyectosApi.csproj", "./"]
RUN dotnet restore "EvaluacionProyectosApi.csproj"
COPY . .

# 3. Construir la aplicación en modo "Release"
RUN dotnet publish "EvaluacionProyectosApi.csproj" -c Release -o /app/publish

# 4. Usar una imagen más ligera solo para correr la app en el servidor de Render
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# 5. Copiar lo que compilamos y encender el motor
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EvaluacionProyectosApi.dll"]