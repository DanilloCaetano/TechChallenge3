FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TechChallenge/RegionsService.csproj", "TechChallenge/"]
COPY ["Infraestructure/Infraestructure.csproj", "Infraestructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "TechChallenge/RegionsService.csproj"
COPY . .
WORKDIR "/src/TechChallenge"
RUN dotnet build "RegionsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RegionsService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "RegionsService.dll"]