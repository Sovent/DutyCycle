#!/bin/bash

dotnet ef database update --project DutyCycle.API --context MigrationsContext --no-build
dotnet test DutyCycle.IntegrationTests --no-build