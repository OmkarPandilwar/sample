FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["src/CustomerManagement.API/CustomerManagement.API.csproj", "src/CustomerManagement.API/"]
COPY ["src/CustomerManagement.Application/CustomerManagement.Application.csproj", "src/CustomerManagement.Application/"]
COPY ["src/CustomerManagement.Domain/CustomerManagement.Domain.csproj", "src/CustomerManagement.Domain/"]
COPY ["src/CustomerManagement.Infrastructure/CustomerManagement.Infrastructure.csproj", "src/CustomerManagement.Infrastructure/"]

RUN dotnet restore "src/CustomerManagement.API/CustomerManagement.API.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/src/CustomerManagement.API"
RUN dotnet build "CustomerManagement.API.csproj" -c Release -o /app/build
RUN dotnet publish "CustomerManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CustomerManagement.API.dll"]