FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
ARG BUILDPLATFORM
WORKDIR /src
COPY ["[project-name]/[project-name].csproj", "[project-name]/"]

ARG TOKEN
ARG SOURCE
RUN dotnet nuget add source -n github "$SOURCE"  -u "admin" -p "$TOKEN" --store-password-in-clear-text

RUN dotnet restore "[project-name]/[project-name].csproj"
COPY . .
WORKDIR "/src/[project-name]"
RUN dotnet build "[project-name].csproj" -c Release -o /app/build -a $TARGETARCH

FROM build AS publish
RUN dotnet publish "[project-name].csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "[project-name].dll"]
