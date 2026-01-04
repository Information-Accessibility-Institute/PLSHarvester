#!/bin/bash
set -e

echo "=== Publishing binaries ==="

dotnet publish PLSHarvester/PLSHarvester.fsproj -c Release -r win-x64    --self-contained true
dotnet publish PLSHarvester/PLSHarvester.fsproj -c Release -r linux-x64  --self-contained true
dotnet publish PLSHarvester/PLSHarvester.fsproj -c Release -r osx-arm64  --self-contained true

echo "=== Preparing dist directories ==="

# 配布ターゲット
OS_LIST=("win-x64" "linux-x64" "osx-arm64")

# 付属ファイル（ソリューション直下）
COMMON_FILES=("README.md" "README_ja.md" "LICENSE" "samples")

# 付属ファイル（プロジェクト直下）
PROJECT_FILES=("PLSHarvester/data/subset.tsv" "PLSHarvester/THIRD-PARTY-NOTICES")

# dist 構築
for OS in "${OS_LIST[@]}"; do
    echo "Preparing dist/${OS} ..."

    mkdir -p dist/${OS}

    # 実行ファイルコピー
    PUBLISH_DIR="PLSHarvester/bin/Release/net8.0/${OS}/publish"

    if [ "$OS" = "win-x64" ]; then
        cp "${PUBLISH_DIR}/PLSHarvester.exe" dist/${OS}/
    else
        cp "${PUBLISH_DIR}/PLSHarvester" dist/${OS}/
    fi

    # 共通ファイルコピー（ルートから）
    for f in "${COMMON_FILES[@]}"; do
        cp -r "$f" dist/${OS}/
    done

    # プロジェクト内のものコピー
    mkdir -p dist/${OS}/data
    cp PLSHarvester/data/subset.tsv dist/${OS}/data/

    cp -r PLSHarvester/THIRD-PARTY-NOTICES dist/${OS}/

done

echo "=== Creating ZIP packages ==="

mkdir -p dist/zips

for OS in "${OS_LIST[@]}"; do
    echo "Zipping for ${OS}..."
    (cd dist && zip -r "zips/PLSHarvester-${OS}.zip" "${OS}")
done

echo "Done."
