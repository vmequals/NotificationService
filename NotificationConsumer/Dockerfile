FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NotificationConsumer/NotificationConsumer.csproj", "NotificationConsumer/"]
RUN dotnet restore "NotificationConsumer/NotificationConsumer.csproj"
COPY . .
WORKDIR "/src/NotificationConsumer"
RUN dotnet build "NotificationConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationConsumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationConsumer.dll"]
