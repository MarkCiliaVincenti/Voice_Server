FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Voice_Server/Voice_Server.csproj", "Voice_Server/"]
RUN dotnet restore "Voice_Server/Voice_Server.csproj"
COPY . .
WORKDIR "/src/Voice_Server"
RUN dotnet build "Voice_Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Voice_Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Voice_Server.dll"]
