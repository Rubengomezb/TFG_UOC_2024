#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TFG_UOC_2024.API/TFG_UOC_2024.API.csproj", "TFG_UOC_2024.API/"]
COPY ["TFG_UOC_2024.CORE/TFG_UOC_2024.CORE.csproj", "TFG_UOC_2024.CORE/"]
COPY ["TFG_UOC_2024.DB/TFG_UOC_2024.DB.csproj", "TFG_UOC_2024.DB/"]
RUN dotnet restore "./TFG_UOC_2024.API/TFG_UOC_2024.API.csproj"
COPY . .
WORKDIR "/src/TFG_UOC_2024.API"
RUN dotnet build "./TFG_UOC_2024.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TFG_UOC_2024.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TFG_UOC_2024.API.dll"]