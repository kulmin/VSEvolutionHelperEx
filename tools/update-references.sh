#!/bin/sh
set -eu

if [ "$#" -ne 2 ]; then
    echo "usage: $0 <native-game-directory> <il2cpp-interop-directory>" >&2
    exit 2
fi

script_dir=$(CDPATH='' cd -- "$(dirname -- "$0")" && pwd)
repo_dir=$(CDPATH='' cd -- "$script_dir/.." && pwd)
mono_source="${1%/}/VampireSurvivors_Data/Managed"
il2cpp_source=${2%/}
tool_dir="$repo_dir/.tools/refasmer"
work_dir=$(mktemp -d)

cleanup() {
    rm -rf -- "$work_dir"
}
trap cleanup EXIT HUP INT TERM

require_file() {
    if [ ! -f "$1" ]; then
        echo "missing reference input: $1" >&2
        exit 1
    fi
}

require_file "$mono_source/VampireSurvivors.Runtime.dll"
require_file "$il2cpp_source/VampireSurvivors.Runtime.dll"
require_file "$il2cpp_source/Il2CppSystem.dll"

if [ ! -x "$tool_dir/refasmer" ]; then
    mkdir -p "$tool_dir"
    dotnet tool install JetBrains.Refasmer.CliTool --tool-path "$tool_dir" --version 2.0.3
fi

mkdir -p "$work_dir/mono" "$work_dir/il2cpp"

mono_files="
Newtonsoft.Json.dll
PauseSystem.dll
PhaserPort.dll
Unity.TextMeshPro.dll
UnityEngine.CoreModule.dll
UnityEngine.dll
UnityEngine.IMGUIModule.dll
UnityEngine.InputLegacyModule.dll
UnityEngine.TextRenderingModule.dll
UnityEngine.UI.dll
UnityEngine.UIModule.dll
VampireSurvivors.Runtime.dll
l2localization.dll
"

il2cpp_files="
Il2Cppmscorlib.dll
Il2CppSystem.dll
Newtonsoft.Json.dll
PauseSystem.dll
PhaserPort.dll
Unity.TextMeshPro.dll
UnityEngine.CoreModule.dll
UnityEngine.IMGUIModule.dll
UnityEngine.InputLegacyModule.dll
UnityEngine.TextRenderingModule.dll
UnityEngine.UI.dll
UnityEngine.UIModule.dll
VampireSurvivors.Runtime.dll
l2localization.dll
"

set --
for file_name in $mono_files; do
    require_file "$mono_source/$file_name"
    set -- "$@" "$mono_source/$file_name"
done
"$tool_dir/refasmer" --public --omit-non-api-members=true -O "$work_dir/mono" "$@"

set --
for file_name in $il2cpp_files; do
    require_file "$il2cpp_source/$file_name"
    set -- "$@" "$il2cpp_source/$file_name"
done
"$tool_dir/refasmer" --all --omit-non-api-members=false -O "$work_dir/il2cpp" "$@"

mkdir -p "$repo_dir/VSEvolutionHelper.BepInEx/lib/mono" "$repo_dir/VSEvolutionHelper.BepInEx/lib/il2cpp"
for file_name in $mono_files; do
    install -m 0644 "$work_dir/mono/$file_name" "$repo_dir/VSEvolutionHelper.BepInEx/lib/mono/$file_name"
done
for file_name in $il2cpp_files; do
    install -m 0644 "$work_dir/il2cpp/$file_name" "$repo_dir/VSEvolutionHelper.BepInEx/lib/il2cpp/$file_name"
done

echo "updated Mono and IL2CPP build references"
