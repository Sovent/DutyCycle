#!/bin/bash

dotnet ef database update --project DutyCycle.API --no-build
dotnet test DutyCycle.IntegrationTests --no-build