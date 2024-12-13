FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TechChallenge/ContactService.csproj", "TechChallenge/"]
COPY ["Infraestructure/Infraestructure.csproj", "Infraestructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "TechChallenge/ContactService.csproj"
COPY . .
WORKDIR "/src/TechChallenge"
RUN dotnet build "ContactService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 7070
ENTRYPOINT ["dotnet", "ContactService.dll"]