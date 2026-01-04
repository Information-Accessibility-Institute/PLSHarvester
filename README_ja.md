# PLSHarvester

PLS Harvester は、日本語の EPUB または HTML ファイルから未知語候補を抽出し、
発音辞書（PLS）のドラフトを生成するツールです。

---

## PLSHarvester とは？

PLSHarvester は、アクセシブルなテキスト読み上げ（TTS）システム向けにPLS（Pronunciation Lexicon Specification）辞書を収集・生成するためのツールです。

主たる目的は、日本語のアクセシビリティ要件に特に配慮しつつ、既存のテキスト資源から高品質な発音辞書を作成する作業を支援することにあります。

---

## 概要

PLSHarvester は以下の入力に対応しています：

- EPUB又はHTMLファイル 
- 既知なので無視すべき語彙の辞書(TSVファイル)

そして次を出力します：

- 漢字からなる既知ではない語彙について `<lexeme>` 要素（`<pronunciation>`要素は空）を入れたPLS辞書（発音情報は後から人間または外部ツールが補完）

---

## 開発状況

本プロジェクトは開発中です。インタフェースとデータ構造は今後変更される可能性があります。

---

## 主な利用用途
  
- スクリーンリーダーやアクセシブルな読書システムにおけるTTSの読み上げ精度向上
- 固有名詞・専門用語・分野特有語彙の適切な読みを確保
- アクセシビリティ、TTS、計算言語学に関する研究・実験用途 

---

## 必要環境

- ランタイム不要（自己完結型の実行ファイルを同梱）
- Windows／macOS／Linux 用のバイナリを提供

---
## 事前ビルド済みバイナリと使用方法

事前にビルドされた ZIP アーカイブを以下から入手できます：

https://github.com/Information-Accessibility-Institute/PLSHarvester/releases

---

### 提供するZIPファイル

- **PLSHarvester-win-x64.zip**（Windows）
- **PLSHarvester-linux-x64.zip**（Linux）
- **PLSHarvester-osx-arm64.zip**（macOS Apple Silicon）

各ZIPファイルは次のものを含みます。

- プラットフォーム固有の実行可能ファイル  
- `data/subset.tsv`  
- `samples/`ディレクトリ  
- `LICENSE`  
- `README.md`  
- `README_ja.md`  
- `THIRD-PARTY-NOTICES/UNIDIC-LICENSE.txt`
---


## 実行方法

### コマンドラインの構文

#### 引数

| 引数 | 意味 |
|---------|---------|
| `INPUT_PATH` | EPUBまたはHTMLファイル |
| `--dict external.tsv` | (省略可能) 既知なので無視すべき語彙の辞書(TSVファイル) |
| `--out out.xml` | (省略可能) 出力PLSファイルのパス (省略時は harvested.pls) |

---

### Windows
```powershell
.\PLSHarvester.exe input.epub --dict data\subset.tsv --out output.pls
```

#### 例:

```powershell
PLSHarvester.exe samples\kusamakura\aozorabunko_00776.epub --dict data\subset.tsv --out kusamakura.pls
```
---

### Linux
実行可能にするには:
```bash
chmod +x PLSHarvester
```
実行:
```bash
./PLSHarvester input.epub --dict external.tsv --out output.pls
```

#### 例:
```bash
./PLSHarvester samples/kusamakura/aozorabunko_00776.epub --dict data/subset.tsv --out out.pls
```

---

### macOS (Apple Silicon)

実行可能にするには:

```bash
chmod +x PLSHarvester
```

Gatekeeperが実行をブロックする場合:

```bash
xattr -d com.apple.quarantine PLSHarvester
```

実行:

```bash
./PLSHarvester input.epub --dict external.tsv --out output.pls
```

#### 例:
```bash
./PLSHarvester samples/kusamakura/aozorabunko_00776.epub --dict data/subset.tsv --out out.pls
```

# サンプル

`samples/` ディレクトリにサンプル入力・出力ファイルを同梱しています。`samples/kusamakura/`には以下のものが含まれています。

- 青空文庫由来のサンプルEPUB
- サンプルPLSファイル
- 処理の流れを説明するREADME

# ライセンス

同梱ファイル:

`LICENSE` — PLSHarvester のライセンス

`THIRD-PARTY-NOTICES/UNIDIC-LICENSE.txt` — UniDic のライセンス

UniDic のデータは、縮小サブセットを生成する目的にのみ使用しています。

# 謝辞

本ツールは、UniDic から生成したサブセットデータを使用しています。
UniDic のライセンス条件については
THIRD-PARTY-NOTICES/UNIDIC-LICENSE.txt を参照してください。

UniDic コンソーシアムおよび青空文庫に感謝いたします。