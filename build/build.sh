#!/bin/bash
set -e

# the package version is the most recent tag reachable from HEAD, without a leading 'v'
VERSION=$(git describe --tags --abbrev=0 2>/dev/null || echo "0.0.0")
VERSION=${VERSION#v}
echo "Version: ${VERSION}"

dotnet restore
dotnet build --no-restore --no-incremental --configuration Release -p:Version="${VERSION}"
