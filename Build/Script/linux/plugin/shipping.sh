#!/usr/bin/env bash
# HorizonQuestDemo - Build ONLY HorizonQuest for Linux
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
source "${SCRIPT_DIR}/../../../../Build/Base/Script/common.sh"

HOST_PLATFORM="${HOST_PLATFORM:-Linux}"
TARGET_PLATFORM="${TARGET_PLATFORM:-Linux}"
TARGET_CONFIGURATION="${TARGET_CONFIGURATION:-Shipping}"

build_find_plugins() {
    local project_root="${1:-$(build_project_root)}"
    if [[ -f "${project_root}/Plugins/HorizonQuest/HorizonQuest.uplugin" ]]; then
        printf '%s\n' "${project_root}/Plugins/HorizonQuest/HorizonQuest.uplugin"
    else
        echo "ERROR: HorizonQuest not found at ${project_root}/Plugins/HorizonQuest/HorizonQuest.uplugin" >&2
        return 1
    fi
}

build_run_plugin "$@"
