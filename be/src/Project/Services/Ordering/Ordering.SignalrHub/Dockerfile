#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Project/Services/Ordering/Ordering.SignalrHub/Ordering.SignalrHub.csproj", "Project/Services/Ordering/Ordering.SignalrHub/"]
RUN dotnet restore "Project/Services/Ordering/Ordering.SignalrHub/Ordering.SignalrHub.csproj"
COPY . .
WORKDIR "/src/Project/Services/Ordering/Ordering.SignalrHub"
RUN dotnet build "Ordering.SignalrHub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.SignalrHub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.SignalrHub.dll"]
