FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.19 AS base
WORKDIR /app
EXPOSE 5047

ENV ASPNETCORE_URLS=http://+:5047

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19 AS build
ARG configuration=Release
WORKDIR /src
COPY ["BlogService.csproj", "./"]
RUN dotnet restore "BlogService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BlogService.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "BlogService.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogService.dll"]
