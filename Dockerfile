# initializing sdk
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS sdk
WORKDIR /src


# restoring
COPY *.sln .
COPY App/*.csproj App/
COPY UnitTester/*.csproj UnitTester/
RUN dotnet restore


# publishing
COPY . .
FROM sdk AS publish
WORKDIR /src
RUN dotnet publish -c Release -o dist


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /src
COPY --from=publish /src/dist .
# ENTRYPOINT ["dotnet", "App.dll"]


# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet App.dll