FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CustomerManagement.API/CustomerManagement.API.csproj", "src/CustomerManagement.API/"]
COPY ["src/CustomerManagement.Application/CustomerManagement.Application.csproj", "src/CustomerManagement.Application/"]
COPY ["src/CustomerManagement.Domain/CustomerManagement.Domain.csproj", "src/CustomerManagement.Domain/"]
COPY ["src/CustomerManagement.Infrastructure/CustomerManagement.Infrastructure.csproj", "src/CustomerManagement.Infrastructure/"]
RUN dotnet restore "src/CustomerManagement.API/CustomerManagement.API.csproj"
COPY . .
WORKDIR "/src/src/CustomerManagement.API"
RUN dotnet build "CustomerManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerManagement.API.dll"]
