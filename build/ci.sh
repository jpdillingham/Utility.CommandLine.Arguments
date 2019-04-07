#!/bin/bash
set -e
__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
__branch=$(git branch --no-color | grep -E '^\*' | awk '{print $2}')

dotnet-sonarscanner begin /key:"jpdillingham_Utility.CommandLine.Arguments" /o:jpdillingham-github /d:sonar.host.url="https://sonarcloud.io" /d:sonar.exclusions="**/*examples*/**" /d:sonar.branch.name=${__branch} /d:sonar.login="${TOKEN_SONARCLOUD}" /d:sonar.cs.opencover.reportsPaths="tests/opencover.xml"

. "${__dir}/build.sh"
. "${__dir}/test.sh"

dotnet-sonarscanner end /d:sonar.login="${TOKEN_SONARCLOUD}"

bash <(curl -s https://codecov.io/bash) -f tests/opencover.xml