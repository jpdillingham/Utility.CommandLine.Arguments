#!/bin/bash
set -e
__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
__branch=$(git branch --no-color | grep -E '^\*' | awk '{print $2}')

export MSYS2_ARG_CONV_EXCL="*"

dotnet-sonarscanner begin \
	/key:"jpdillingham_Utility.CommandLine.Arguments" \
	/o:jpdillingham-github \
	/d:sonar.host.url="https://sonarcloud.io" \
	/d:sonar.exclusions="**/*examples*/**" \
	/d:sonar.branch.name=${__branch} \
	/d:sonar.cs.opencover.reportsPaths="tests/opencover.xml" \
	/d:sonar.login="${TOKEN_SONARCLOUD}" \

. "${__dir}/build.sh"
. "${__dir}/test.sh"

dotnet-sonarscanner end /d:sonar.login="${TOKEN_SONARCLOUD}"

bash <(curl -s https://codecov.io/bash) -f tests/opencover.xml