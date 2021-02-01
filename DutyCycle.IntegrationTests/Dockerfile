FROM mcr.microsoft.com/dotnet/sdk:3.1

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /usr/app
COPY . .

RUN dotnet build
RUN chmod +x ./DutyCycle.IntegrationTests/run_tests.sh

CMD ["./DutyCycle.IntegrationTests/run_tests.sh"]