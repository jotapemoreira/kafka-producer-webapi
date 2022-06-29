FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV TZ=America/Sao_Paulo
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

ENV TZ=America/Sao_Paulo
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Kafka.Producer.WebApi.dll"]