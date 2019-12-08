FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY /src .
RUN dotnet restore "Bijector.Accounts.csproj"
RUN dotnet build "Bijector.Accounts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bijector.Accounts.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bijector.Accounts.dll"]
