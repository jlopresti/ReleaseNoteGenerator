#!/bin/bash
set -ev
dotnet restore src
dotnet test src/Ranger.NetCore.Tests/Ranger.NetCore.Tests.csproj
dotnet build src -c Release