#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Project/Web/Seller/Seller.Web/Seller.Web.csproj", "Project/Web/Seller/Seller.Web/"]
COPY ["Infrastructure/Account/Foundation.Account/Foundation.Account.csproj", "Infrastructure/Account/Foundation.Account/"]
COPY ["Infrastructure/ApiExtensions/Foundation.ApiExtensions/Foundation.ApiExtensions.csproj", "Infrastructure/ApiExtensions/Foundation.ApiExtensions/"]
COPY ["Infrastructure/Localization/Foundation.Localization/Foundation.Localization.csproj", "Infrastructure/Localization/Foundation.Localization/"]
COPY ["Infrastructure/Extensions/Foundation.Extensions/Foundation.Extensions.csproj", "Infrastructure/Extensions/Foundation.Extensions/"]
COPY ["Infrastructure/Security/Foundation.Security/Foundation.Security.csproj", "Infrastructure/Security/Foundation.Security/"]
COPY ["Infrastructure/Prerendering/Foundation.Prerendering/Foundation.Prerendering.csproj", "Infrastructure/Prerendering/Foundation.Prerendering/"]
COPY ["Infrastructure/Presentation/Foundation.Presentation/Foundation.Presentation.csproj", "Infrastructure/Presentation/Foundation.Presentation/"]
COPY ["Infrastructure/PageContent/Foundation.PageContent/Foundation.PageContent.csproj", "Infrastructure/PageContent/Foundation.PageContent/"]
COPY ["Infrastructure/GenericRepository/Foundation.GenericRepository/Foundation.GenericRepository.csproj", "Infrastructure/GenericRepository/Foundation.GenericRepository/"]
RUN dotnet restore "Project/Web/Seller/Seller.Web/Seller.Web.csproj"
COPY . .
WORKDIR "/src/Project/Web/Seller/Seller.Web"
RUN dotnet build "Seller.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seller.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seller.Web.dll"]
