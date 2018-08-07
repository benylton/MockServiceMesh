FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ApiMockServiceMesh/ApiMockServiceMesh.csproj ApiMockServiceMesh/
RUN dotnet restore ApiMockServiceMesh/ApiMockServiceMesh.csproj
COPY . .
WORKDIR /src/ApiMockServiceMesh.csproj
RUN dotnet build ApiMockServiceMesh.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ApiMockServiceMesh.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiMockServiceMesh.dll"]
