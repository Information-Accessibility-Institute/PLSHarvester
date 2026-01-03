# PLSHarvester

PLSHarvester is a tool for harvesting and generating PLS
(Pronunciation Lexicon Specification) lexicons for accessible
text-to-speech (TTS) systems.

The primary goal of this project is to support the creation of
high-quality pronunciation lexicons from existing linguistic
resources, with a particular focus on Japanese accessibility
requirements.

---

## Overview

PLSHarvester processes linguistic data (e.g. TSV files derived from
morphological dictionaries such as UniDic) and generates PLS-compliant
lexicons that can be used by screen readers and other TTS engines.

Typical use cases include:

- Building pronunciation lexicons for accessible reading systems
- Supporting correct reading of proper names, technical terms, and
  domain-specific vocabulary
- Research and experimentation related to accessibility, TTS, and
  spoken language processing

---

## Status

This project is currently under active development.
Interfaces, data formats, and internal structures may change.

---

## License

This project is licensed under the MIT License.
See the `LICENSE` file for details.

---

## Third-party data and licenses

This project includes data derived from UniDic.
The UniDic license is provided in `third-party-notices\UNIDIC-LICENSE.txt`.


# PLSHarvester（日本語）

PLSHarvester は、アクセシブルな音声合成（TTS）のための  
PLS（Pronunciation Lexicon Specification）辞書を生成・収集する
ツールです。

既存の言語資源を活用して、高品質な発音辞書を作成することを
目的としており、特に **日本語アクセシビリティ** の要件を
重視しています。

---

## 概要

PLSHarvester は、UniDic などの形態素辞書から派生した
TSV 形式のデータを入力として処理し、PLS 仕様に準拠した
発音辞書を生成します。

想定される用途は次のとおりです。

- アクセシブルな読書・閲覧システム向けの発音辞書作成
- 固有名詞、専門用語、分野固有語彙の正確な読みの支援
- アクセシビリティ、音声合成、音声言語処理に関する研究・検証

---

## 開発状況

本プロジェクトは現在開発中です。  
インタフェースやデータ形式、内部構造は変更される可能性があります。

---

## ライセンス

本プロジェクトは MIT ライセンスの下で公開されています。  
詳細は `LICENSE` ファイルを参照してください。

---

## サードパーティデータおよびライセンス

本プロジェクトには UniDic に由来するデータが含まれています。  
UniDic のライセンス文書は、`third-party-notices\UNIDIC-LICENSE.txt` に収録されています。
