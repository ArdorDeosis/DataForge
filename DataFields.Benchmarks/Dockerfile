FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DataFields.Benchmarks/DataFields.Benchmarks.csproj", "DataFields.Benchmarks/"]
RUN dotnet restore "DataFields.Benchmarks/DataFields.Benchmarks.csproj"
COPY . .
WORKDIR "/src/DataFields.Benchmarks"
RUN dotnet build "DataFields.Benchmarks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataFields.Benchmarks.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataFields.Benchmarks.dll"]
