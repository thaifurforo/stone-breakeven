FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . /src/
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "AccountManagementService.Api/AccountManagementService.Api.csproj"

WORKDIR "/src/AccountManagementService.Api"
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
		dotnet build "AccountManagementService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
		dotnet publish "AccountManagementService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountManagementService.Api.dll"]