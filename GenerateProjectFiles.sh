#!/bin/sh
set -e
PROJECT_ROOT=$(pwd)
source ue_ci_scripts/function/sh/public/ue_build_function.sh

PROJECT_FILE_NAME=$(find *.uproject)
PROJECT_NAME=${PROJECT_FILE_NAME%.*}

CURRENT_FOLDER=$(pwd)
cmd=" \
	 '${UNREAL_ENGINE_ROOT}/Engine/Binaries/DotNET/UnrealBuildTool.exe' -projectfiles  \
		 -project='${CURRENT_FOLDER}/${PROJECT_NAME}.uproject' -game -rocket -progress
	 "
echo ${cmd}
eval ${cmd}