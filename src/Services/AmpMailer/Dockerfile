#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/AmpMailer/AmpMailer.csproj", "Services/AmpMailer/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "Services/AmpMailer/AmpMailer.csproj"
COPY . .
WORKDIR "/src/Services/AmpMailer"
RUN dotnet build "AmpMailer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AmpMailer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AmpMailer.dll"]