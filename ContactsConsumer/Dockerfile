FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ContactConsumer/ContactConsumer.csproj", "ContactConsumer/"]
COPY ["Infraestructure/Infraestructure.csproj", "Infraestructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "ContactConsumer/ContactConsumer.csproj"
COPY . .
WORKDIR "/src/ContactConsumer"
RUN dotnet build "ContactConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 6060
ENTRYPOINT ["dotnet", "ContactConsumer.dll"]