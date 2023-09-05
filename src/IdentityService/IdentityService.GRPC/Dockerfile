#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/IdentityService/IdentityService.GRPC/IdentityService.GRPC.csproj", "src/IdentityService/IdentityService.GRPC/"]
COPY ["src/IdentityService/IdentityService.Infrastructure/IdentityService.Infrastructure.csproj", "src/IdentityService/IdentityService.Infrastructure/"]
COPY ["src/IdentityService/IdentityService.Application/IdentityService.Application.csproj", "src/IdentityService/IdentityService.Application/"]
COPY ["src/IdentityService/IdentityService.Domain/IdentityService.Domain.csproj", "src/IdentityService/IdentityService.Domain/"]
RUN dotnet restore "src/IdentityService/IdentityService.GRPC/IdentityService.GRPC.csproj"
COPY . .
WORKDIR "/src/src/IdentityService/IdentityService.GRPC"
RUN dotnet build "IdentityService.GRPC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityService.GRPC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.GRPC.dll"]