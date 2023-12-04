#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GloboTicket.Client/GloboTicket.Web.csproj", "GloboTicket.Client/"]
COPY ["GloboTicket.Integration.Messages/GloboTicket.Integration.Messages.csproj", "GloboTicket.Integration.Messages/"]
RUN dotnet restore "GloboTicket.Client/GloboTicket.Web.csproj"
COPY . .
WORKDIR "/src/GloboTicket.Client"
RUN dotnet build "GloboTicket.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GloboTicket.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GloboTicket.Web.dll"]