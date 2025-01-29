FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CineDigital.csproj", "./"]
RUN dotnet restore "./CineDigital.csproj"

COPY . .
RUN dotnet publish "CineDigital.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "CineDigital.dll"]