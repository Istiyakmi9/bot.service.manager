#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["bot.service.manager/bot.service.manager.csproj", "bot.service.manager/"]
RUN dotnet restore "bot.service.manager/bot.service.manager.csproj"
COPY . .
WORKDIR "/src/bot.service.manager"
RUN dotnet build "bot.service.manager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "bot.service.manager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bot.service.manager.dll"]