#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Project/Services/Media/Media.Api/Media.Api.csproj", "Project/Services/Media/Media.Api/"]
COPY ["Infrastructure/Account/Foundation.Account/Foundation.Account.csproj", "Infrastructure/Account/Foundation.Account/"]
COPY ["Infrastructure/ApiExtensions/Foundation.ApiExtensions/Foundation.ApiExtensions.csproj", "Infrastructure/ApiExtensions/Foundation.ApiExtensions/"]
COPY ["Infrastructure/Localization/Foundation.Localization/Foundation.Localization.csproj", "Infrastructure/Localization/Foundation.Localization/"]
COPY ["Infrastructure/EventBus/Foundation.EventBusRabbitMq/Foundation.EventBusRabbitMq.csproj", "Infrastructure/EventBus/Foundation.EventBusRabbitMq/"]
COPY ["Infrastructure/EventBus/Foundation.EventBus/Foundation.EventBus.csproj", "Infrastructure/EventBus/Foundation.EventBus/"]
COPY ["Infrastructure/Extensions/Foundation.Extensions/Foundation.Extensions.csproj", "Infrastructure/Extensions/Foundation.Extensions/"]
COPY ["Infrastructure/GenericRepository/Foundation.GenericRepository/Foundation.GenericRepository.csproj", "Infrastructure/GenericRepository/Foundation.GenericRepository/"]
RUN dotnet restore "Project/Services/Media/Media.Api/Media.Api.csproj"
COPY . .
WORKDIR "/src/Project/Services/Media/Media.Api"
RUN dotnet build "Media.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Media.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Media.Api.dll"]