#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
# EXPOSE 80
# EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GloboTicket.Services.Discount/GloboTicket.Services.Discount.csproj", "GloboTicket.Services.Discount/"]
RUN dotnet restore "GloboTicket.Services.Discount/GloboTicket.Services.Discount.csproj"
COPY . .
WORKDIR "/src/GloboTicket.Services.Discount"
RUN dotnet build "GloboTicket.Services.Discount.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GloboTicket.Services.Discount.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5007
ENTRYPOINT ["dotnet", "GloboTicket.Services.Discount.dll"]