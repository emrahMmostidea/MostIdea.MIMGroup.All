#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/MostIdea.MIMGroup.Migrator/MostIdea.MIMGroup.Migrator.csproj", "src/MostIdea.MIMGroup.Migrator/"]
COPY ["src/MostIdea.MIMGroup.EntityFrameworkCore/MostIdea.MIMGroup.EntityFrameworkCore.csproj", "src/MostIdea.MIMGroup.EntityFrameworkCore/"]
COPY ["src/MostIdea.MIMGroup.Core/MostIdea.MIMGroup.Core.csproj", "src/MostIdea.MIMGroup.Core/"]
COPY ["src/MostIdea.MIMGroup.Core.Shared/MostIdea.MIMGroup.Core.Shared.csproj", "src/MostIdea.MIMGroup.Core.Shared/"]
RUN dotnet restore "src/MostIdea.MIMGroup.Migrator/MostIdea.MIMGroup.Migrator.csproj"
COPY . .
WORKDIR "/src/src/MostIdea.MIMGroup.Migrator"
RUN dotnet build "MostIdea.MIMGroup.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MostIdea.MIMGroup.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MostIdea.MIMGroup.Migrator.dll"]
