#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/HiringService/HiringService.API/HiringService.API.csproj", "src/HiringService/HiringService.API/"]
COPY ["src/HiringService/HiringService.Infrastructure/HiringService.Infrastructure.csproj", "src/HiringService/HiringService.Infrastructure/"]
COPY ["src/HiringService/HiringService.Application/HiringService.Application.csproj", "src/HiringService/HiringService.Application/"]
COPY ["src/HiringService/HiringService.Domain/HiringService.Domain.csproj", "src/HiringService/HiringService.Domain/"]
RUN dotnet restore "src/HiringService/HiringService.API/HiringService.API.csproj"
COPY . .
WORKDIR "/src/src/HiringService/HiringService.API"
RUN dotnet build "HiringService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HiringService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HiringService.API.dll"]