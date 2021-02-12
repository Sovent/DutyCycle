FROM sovent/dutycyclebase

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

CMD ["dotnet", "ef", "database", "update", "--project", "DutyCycle.API", "--no-build"]