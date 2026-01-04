# PLSHarvester

（日本語・簡易説明）  
日本語 EPUB または HTML から語彙候補を抽出し、PLS 辞書（草案）を生成するツールです。

(English overview)  
PLS Harvester extracts candidate unknown words from Japanese EPUB or HTML files  and generates a draft Pronunciation Lexicon (PLS) dictionary.

**Full Japanese documentation:** README_ja.md

---

## What is PLSHarvester?

PLSHarvester is a tool for harvesting and generating PLS (Pronunciation Lexicon Specification) dictionaries for use in accessible text-to-speech (TTS) systems.

Its primary purpose is to assist in producing high-quality pronunciation lexicons from existing textual resources, with a special focus on Japanese accessibility requirements.

---

## Overview

PLSHarvester supports the following inputs:

- An EPUB or HTML file
- A TSV dictionary of known words to be excluded

And it produces the following output:

- A PLS dictionary containing `<lexeme>` entries for Kanji-based words that are not already known, with empty `<pronunciation>` elements (to be filled by human editors or downstream tools)


---

## Status

This project is under active development.  Interfaces and data structures may change.

---

## Typical Use Cases

- Improving TTS accuracy for screen readers and accessible reading systems  
- Ensuring proper reading of proper names, technical terms, and domain-specific vocabulary  
- Research and experimentation in accessibility, TTS, and computational linguistics  

---

## Requirements

- No runtime required (self-contained executables provided)
- Binaries included for Windows, macOS, and Linux

---

## Pre-built Binaries and Usage Guide

Pre-built ZIP archives are available:

https://github.com/Information-Accessibility-Institute/PLSHarvester/releases

### Provided ZIP files

- **PLSHarvester-win-x64.zip**（Windows）
- **PLSHarvester-linux-x64.zip**（Linux）
- **PLSHarvester-osx-arm64.zip**（macOS Apple Silicon）

Each ZIP contains:

- Platform-specific executable  
- `data/subset.tsv`  
- `samples/` directory  
- `LICENSE`  
- `README.md`  
- `README_ja.md`  
- `THIRD-PARTY-NOTICES/UNIDIC-LICENSE.txt`

---

## How to Run

### **Command-line Syntax**


#### Arguments

| Argument | Meaning |
|---------|---------|
| `INPUT_PATH` | EPUB or HTML file |
| `--dict external.tsv` | (Optional) an additional TSV dictionary to *exclude* words already known |
| `--out out.xml` | (Optional) output PLS file path (default: harvested.pls) |

---

### Windows
```powershell
.\PLSHarvester.exe input.epub --dict data\subset.tsv --out output.pls
```

#### Example:

```powershell
PLSHarvester.exe samples\kusamakura\aozorabunko_00776.epub --dict data\subset.tsv --out kusamakura.pls
```
---

### Linux
Make executable:
```bash
chmod +x PLSHarvester
```
Run:
```bash
./PLSHarvester input.epub --dict external.tsv --out output.pls
```

#### Example:
```bash
./PLSHarvester samples/kusamakura/aozorabunko_00776.epub --dict data/subset.tsv --out out.pls
```

---

### macOS (Apple Silicon)

Make executable:

```bash
chmod +x PLSHarvester
```

If Gatekeeper blocks execution:

```bash
xattr -d com.apple.quarantine PLSHarvester
```

Run:

```bash
./PLSHarvester input.epub --dict external.tsv --out output.pls
```

#### Example:
```bash
./PLSHarvester samples/kusamakura/aozorabunko_00776.epub --dict data/subset.tsv --out out.pls
```

# Samples

The `samples/`directory contains sample inputs and outputs.
`samples/kusamakura/` contains:

- Example EPUB (from Aozora Bunko)
- Example PLS file
- README explaining processing flow

# License

Included files:

`LICENSE` — License for PLSHarvester

`THIRD-PARTY-NOTICES/UNIDIC-LICENSE.txt` — UniDic license

UniDic data are used only for generating a reduced subset.

# Acknowledgements

This tool uses subset data derived from UniDic.
UniDic is licensed under the conditions in
THIRD-PARTY-NOTICES/UNIDIC-LICENSE.txt.

We thank the UniDic Consortium and Aozora Bunko.