FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy project file first so Docker can cache package restore separately.
COPY ["apps/channel-management-microservice/src/ChannelManagement.csproj", "apps/channel-management-microservice/src/"]
COPY ["packages/Shared.Contracts/Shared.Contracts.csproj", "packages/Shared.Contracts/"]
RUN dotnet restore "apps/channel-management-microservice/src/ChannelManagement.csproj"

# Copy the service source and compile it.
WORKDIR "/src"
COPY ["apps/channel-management-microservice/src/", "apps/channel-management-microservice/src/"]
COPY ["packages/Shared.Contracts/", "packages/Shared.Contracts/"]
WORKDIR "apps/channel-management-microservice/src/"
RUN dotnet build "ChannelManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChannelManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChannelManagement.dll"]
