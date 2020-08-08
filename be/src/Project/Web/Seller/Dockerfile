#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Project/Web/Seller/Seller.Web.csproj", "Project/Web/Seller/"]
COPY ["Infrastructure/Presentation/Foundation.Presentation.csproj", "Infrastructure/Presentation/"]
COPY ["Infrastructure/PageContent/Foundation.PageContent.csproj", "Infrastructure/PageContent/"]
COPY ["Infrastructure/Localization/Foundation.Localization.csproj", "Infrastructure/Localization/"]
COPY ["Infrastructure/Extensions/Foundation.Extensions.csproj", "Infrastructure/Extensions/"]
COPY ["Infrastructure/Security/Foundation.Security.csproj", "Infrastructure/Security/"]
COPY ["Infrastructure/ApiExtensions/Foundation.ApiExtensions.csproj", "Infrastructure/ApiExtensions/"]
COPY ["Infrastructure/GenericRepository/Foundation.GenericRepository.csproj", "Infrastructure/GenericRepository/"]
COPY ["Infrastructure/Prerendering/Foundation.Prerendering.csproj", "Infrastructure/Prerendering/"]
COPY ["Infrastructure/Account/Foundation.Account.csproj", "Infrastructure/Account/"]
RUN dotnet restore "Project/Web/Seller/Seller.Web.csproj"
COPY . .
WORKDIR "/src/Project/Web/Seller"
RUN dotnet build "Seller.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seller.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seller.Web.dll"]
