#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Desafio_Nubank/Desafio_Nubank.csproj", "Desafio_Nubank/"]
RUN dotnet restore "Desafio_Nubank/Desafio_Nubank.csproj"
COPY . .
WORKDIR "/src/Desafio_Nubank"
RUN dotnet build "Desafio_Nubank.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Desafio_Nubank.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Desafio_Nubank.dll"]