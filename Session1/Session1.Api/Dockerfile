FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Session1.Api/Session1.Api.csproj", "Session1.Api/"]
RUN dotnet restore "Session1.Api/Session1.Api.csproj"
COPY . .
WORKDIR "/src/Session1.Api"
RUN dotnet build "Session1.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Session1.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Session1.Api.dll"]