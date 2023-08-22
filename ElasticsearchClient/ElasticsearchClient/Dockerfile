FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ElasticsearchClient/ElasticsearchClient.csproj", "ElasticsearchClient/"]
RUN dotnet restore "ElasticsearchClient/ElasticsearchClient.csproj"
COPY . .
WORKDIR "/src/ElasticsearchClient"
RUN dotnet build "ElasticsearchClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ElasticsearchClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElasticsearchClient.dll"]
