#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["API/API.csproj", "API/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Services/Services.csproj", "Services/"]
RUN dotnet restore "API/API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]

# Copy wait-for-it.sh into our image
COPY wait-for-it.sh wait-for-it.sh
# Make it executable, in Linux
RUN chmod +x wait-for-it.sh