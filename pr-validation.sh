#!/bin/bash

echo "Simulating PR validation..."

# Restore, build, test
dotnet restore
dotnet build --no-restore
dotnet test --no-build

echo "Validation complete (no deploy)."