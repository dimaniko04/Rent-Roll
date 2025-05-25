FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# restore
COPY src/RentnRoll.Api/*.csproj ./RentnRoll.Api/
COPY src/RentnRoll.Application/*.csproj ./RentnRoll.Application/
COPY src/RentnRoll.Domain/*.csproj ./RentnRoll.Domain/
COPY src/RentnRoll.Persistence/*.csproj ./RentnRoll.Persistence/
RUN dotnet restore "RentnRoll.Api/RentnRoll.Api.csproj"

# build
COPY . ../
WORKDIR /src/RentnRoll.Api
RUN dotnet build "RentnRoll.Api.csproj" -c release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish -c release -o /app/publish --no-restore

# run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
USER app
ENV ASPNETCORE_HTTP_PORTS=5000
ENV ASPNETCORE_HTTPS_PORTS=5001
EXPOSE 5000
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "RentnRoll.Api.dll" ]
