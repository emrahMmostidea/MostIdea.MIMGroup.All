#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/MostIdea.MIMGroup.Web.Public/MostIdea.MIMGroup.Web.Public.csproj", "src/MostIdea.MIMGroup.Web.Public/"]
COPY ["src/MostIdea.MIMGroup.Web.Core/MostIdea.MIMGroup.Web.Core.csproj", "src/MostIdea.MIMGroup.Web.Core/"]
COPY ["src/MostIdea.MIMGroup.Application/MostIdea.MIMGroup.Application.csproj", "src/MostIdea.MIMGroup.Application/"]
COPY ["src/MostIdea.MIMGroup.Application.Shared/MostIdea.MIMGroup.Application.Shared.csproj", "src/MostIdea.MIMGroup.Application.Shared/"]
COPY ["src/MostIdea.MIMGroup.Core.Shared/MostIdea.MIMGroup.Core.Shared.csproj", "src/MostIdea.MIMGroup.Core.Shared/"]
COPY ["src/MostIdea.MIMGroup.Core/MostIdea.MIMGroup.Core.csproj", "src/MostIdea.MIMGroup.Core/"]
COPY ["src/MostIdea.MIMGroup.EntityFrameworkCore/MostIdea.MIMGroup.EntityFrameworkCore.csproj", "src/MostIdea.MIMGroup.EntityFrameworkCore/"]
COPY ["src/MostIdea.MIMGroup.GraphQL/MostIdea.MIMGroup.GraphQL.csproj", "src/MostIdea.MIMGroup.GraphQL/"]
RUN dotnet restore "src/MostIdea.MIMGroup.Web.Public/MostIdea.MIMGroup.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/MostIdea.MIMGroup.Web.Public"
RUN dotnet build "MostIdea.MIMGroup.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MostIdea.MIMGroup.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MostIdea.MIMGroup.Web.Public.dll"]
