#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ProjectManagementService/ProjectManagementService.API/ProjectManagementService.API.csproj", "src/ProjectManagementService/ProjectManagementService.API/"]
COPY ["src/ProjectManagementService/ProjectManagementService.Infrastructure/ProjectManagementService.Infrastucture.csproj", "src/ProjectManagementService/ProjectManagementService.Infrastructure/"]
COPY ["src/ProjectManagementService/ProjectManagementService.Application/ProjectManagementService.Application.csproj", "src/ProjectManagementService/ProjectManagementService.Application/"]
COPY ["src/ProjectManagementService/ProjectManagementService.Domain/ProjectManagementService.Domain.csproj", "src/ProjectManagementService/ProjectManagementService.Domain/"]
RUN dotnet restore "src/ProjectManagementService/ProjectManagementService.API/ProjectManagementService.API.csproj"
COPY . .
WORKDIR "/src/src/ProjectManagementService/ProjectManagementService.API"
RUN dotnet build "ProjectManagementService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectManagementService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectManagementService.API.dll"]