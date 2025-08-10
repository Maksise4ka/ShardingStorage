FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source
COPY . ./src
COPY *.sln .

WORKDIR /source/src/ShardingStorage.Orders.WebApi
RUN dotnet restore

WORKDIR /source/src/ShardingStorage.Orders.WebApi
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ShardingStorage.Orders.WebApi.dll"]