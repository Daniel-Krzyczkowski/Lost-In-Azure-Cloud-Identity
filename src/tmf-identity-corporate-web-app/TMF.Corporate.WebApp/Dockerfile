#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["TMF.Corporate.WebApp/TMF.Corporate.WebApp.csproj", "TMF.Corporate.WebApp/"]
RUN dotnet restore "TMF.Corporate.WebApp/TMF.Corporate.WebApp.csproj"
COPY . .
WORKDIR "/src/TMF.Corporate.WebApp"
RUN dotnet build "TMF.Corporate.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TMF.Corporate.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TMF.Corporate.WebApp.dll"]