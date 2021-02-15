FROM mcr.microsoft.com/dotnet/sdk:3.1 as base

WORKDIR /usr/app
COPY ./src/DutyCycle ./DutyCycle
COPY ./src/DutyCycle.API ./DutyCycle.API

RUN dotnet publish -c Release -o Deploy DutyCycle.API

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /usr/app
COPY --from=base /usr/app/Deploy ./

ENTRYPOINT ["dotnet", "DutyCycle.API.dll"]