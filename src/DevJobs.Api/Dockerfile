FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_HTTP_PORTS =	8080
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DevJobs.Api/DevJobs.Api.csproj", "src/DevJobs.Api/"]
COPY ["src/DevJobs.Application/DevJobs.Application.csproj", "src/DevJobs.Application/"]
COPY ["src/DevJobs.Domain/DevJobs.Domain.csproj", "src/DevJobs.Domain/"]
COPY ["src/DevJobs.Infrastructure/DevJobs.Infrastructure.csproj", "src/DevJobs.Infrastructure/"]
RUN dotnet restore "./src/DevJobs.Api/./DevJobs.Api.csproj"
COPY . .
WORKDIR "/src/src/DevJobs.Api"
RUN dotnet build "./DevJobs.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DevJobs.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevJobs.Api.dll", "--environment=Development"]