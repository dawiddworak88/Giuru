#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Project/Services/Catalog/Catalog.BackgroundTasks/Catalog.BackgroundTasks.csproj", "Project/Services/Catalog/Catalog.BackgroundTasks/"]
COPY ["Infrastructure/Catalog/Foundation.Catalog/Foundation.Catalog.csproj", "Infrastructure/Catalog/Foundation.Catalog/"]
COPY ["Infrastructure/GenericRepository/Foundation.GenericRepository/Foundation.GenericRepository.csproj", "Infrastructure/GenericRepository/Foundation.GenericRepository/"]
COPY ["Infrastructure/Search/Foundation.Search/Foundation.Search.csproj", "Infrastructure/Search/Foundation.Search/"]
COPY ["Infrastructure/Extensions/Foundation.Extensions/Foundation.Extensions.csproj", "Infrastructure/Extensions/Foundation.Extensions/"]
COPY ["Infrastructure/Localization/Foundation.Localization/Foundation.Localization.csproj", "Infrastructure/Localization/Foundation.Localization/"]
COPY ["Infrastructure/EventBus/Foundation.EventBusRabbitMq/Foundation.EventBusRabbitMq.csproj", "Infrastructure/EventBus/Foundation.EventBusRabbitMq/"]
COPY ["Infrastructure/EventBus/Foundation.EventBus/Foundation.EventBus.csproj", "Infrastructure/EventBus/Foundation.EventBus/"]
RUN dotnet restore "Project/Services/Catalog/Catalog.BackgroundTasks/Catalog.BackgroundTasks.csproj"
COPY . .
WORKDIR "/src/Project/Services/Catalog/Catalog.BackgroundTasks"
RUN dotnet build "Catalog.BackgroundTasks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.BackgroundTasks.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.BackgroundTasks.dll"]