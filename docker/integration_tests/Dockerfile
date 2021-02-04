FROM mcr.microsoft.com/dotnet/sdk:3.1

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /usr/app
COPY ./src .

RUN dotnet build DutyCycle.sln

COPY ./docker/integration_tests/run_tests.sh .
RUN chmod +x run_tests.sh

CMD ["./run_tests.sh"]