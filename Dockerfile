FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Roupa.Api/Roupa.Api.csproj", "src/Roupa.Api/"]
COPY ["src/Roupa.Application/Roupa.Application.csproj", "src/Roupa.Application/"]
COPY ["src/Roupa.Domain/Roupa.Domain.csproj", "src/Roupa.Domain/"]
COPY ["src/Roupa.Infrastructure/Roupa.Infrastructure.csproj", "src/Roupa.Infrastructure/"]
RUN dotnet restore "src/Roupa.Api/Roupa.Api.csproj"
COPY . .
RUN dotnet publish "src/Roupa.Api/Roupa.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Roupa.Api.dll"]
