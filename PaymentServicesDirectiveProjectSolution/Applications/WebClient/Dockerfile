#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Applications/WebClient/Applications.WebClient.csproj", "Applications/WebClient/"]

RUN dotnet restore "Applications/WebClient/Applications.WebClient.csproj"

COPY . .
WORKDIR "/PaymentServicesDirectiveProjectSolution/Applications/WebClient/Applications.WebClient"
RUN dotnet build "Applications.WebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Applications.WebClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Staging

ENTRYPOINT ["dotnet", "Applications.WebClient.dll"]