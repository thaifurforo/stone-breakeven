FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AccountingService.Api/AccountingService.Api.csproj", "AccountingService.Api/"]
RUN dotnet restore "AccountingService.Api/AccountingService.Api.csproj"
COPY . .
WORKDIR "/src/AccountingService.Api"
RUN dotnet build "AccountingService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccountingService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountingService.Api.dll"]
