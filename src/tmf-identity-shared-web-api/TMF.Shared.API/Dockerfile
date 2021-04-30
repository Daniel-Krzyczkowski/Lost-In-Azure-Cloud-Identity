#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["TMF.Shared.API/TMF.Shared.API.csproj", "TMF.Shared.API/"]
RUN dotnet restore "TMF.Shared.API/TMF.Shared.API.csproj"
COPY . .
WORKDIR "/src/TMF.Shared.API"
RUN dotnet build "TMF.Shared.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TMF.Shared.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TMF.Shared.API.dll"]