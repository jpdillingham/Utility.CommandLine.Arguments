#!/bin/bash
set -e

dotnet test --no-build --configuration Release -p:CollectCoverage=true -p:CoverletOutput="../opencover.xml" -p:CoverletOutputFormat=opencover tests/Utility.CommandLine.Arguments.Tests -p:Include="[Utility.CommandLine.Arguments*]*" -p:Exclude="[*.Tests]*"