FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# restore
COPY *.sln .
COPY RentnRoll.API/*.csproj ./RentnRoll.API/
COPY RentnRoll.Application/*.csproj ./RentnRoll.Application/
COPY RentnRoll.Core/*.csproj ./RentnRoll.Core/
COPY RentnRoll.Persistence/*.csproj ./RentnRoll.Persistence/
RUN dotnet restore "RentnRoll.API/RentnRoll.API.csproj"

# build
COPY . .
WORKDIR /src
RUN dotnet build -c release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish -c release -o /app/publish --no-restore

# run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
USER app
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "RentnRoll.API.dll" ]
