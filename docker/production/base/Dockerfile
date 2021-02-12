FROM mcr.microsoft.com/dotnet/sdk:3.1

WORKDIR /usr/app
COPY ./src/DutyCycle ./DutyCycle
COPY ./src/DutyCycle.API ./DutyCycle.API

RUN dotnet build -c Release DutyCycle.API

RUN dotnet publish -c Release -o Deploy --no-build DutyCycle.API