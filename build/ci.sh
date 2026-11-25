#!/bin/bash
set -e
__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

export MSYS2_ARG_CONV_EXCL="*"

# SonarCloud analysis runs only when a token is available (it is not for pull requests from forks).
# Branch and pull request details are detected automatically from the GitHub Actions environment.
if [ -n "${TOKEN_SONARCLOUD}" ]; then
	dotnet-sonarscanner begin \
		/k:"jpdillingham_Utility.CommandLine.Arguments" \
		/o:jpdillingham-github \
		/d:sonar.host.url="https://sonarcloud.io" \
		/d:sonar.exclusions="**/*examples*/**" \
		/d:sonar.cs.opencover.reportsPaths="tests/opencover.xml" \
		/d:sonar.token="${TOKEN_SONARCLOUD}"
fi

. "${__dir}/build.sh"
. "${__dir}/test.sh"

if [ -n "${TOKEN_SONARCLOUD}" ]; then
	dotnet-sonarscanner end /d:sonar.token="${TOKEN_SONARCLOUD}"
fi
