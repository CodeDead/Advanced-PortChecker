# Advanced PortChecker

![Advanced PortChecker](https://i.imgur.com/vdt1sXZ.png)

[English](README.md) | [简体中文](README.zh-CN.md) | [Italiano](README.it.md) | [日本語](README.ja.md)

![GitHub](https://img.shields.io/badge/language-JavaScript+Rust-green)
![GitHub](https://img.shields.io/github/license/CodeDead/Advanced-PortChecker)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/CodeDead/Advanced-PortChecker)
[![Test](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/test.yml/badge.svg)](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/test.yml)
[![Release](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/release.yml/badge.svg)](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/release.yml)

Advanced PortCheckerは、特定のホストでポートが開いているか閉じているかを確認できる無料のオープンソースアプリケーションです。複数のポートを一度にチェックし、結果をプレーンテキスト、CSV、またはJSON形式でエクスポートできます！

## ビルド

Advanced PortCheckerは、デスクトップアプリケーションをビルドするためにtauriを使用しています。Tauriの詳細については[こちら](https://tauri.app/v1/guides/)をご覧ください。

`vite`を使用したビルドの詳細については、[こちら](https://vitejs.dev/guide/build.html)の`Vite`ドキュメントをお読みください。

### 開発

`yarn tdev`コマンドを実行して、開発プレビューを開始できます：
```shell
yarn tdev
```

### Windows

#### インストーラー

Windowsホストで`yarn tbuild`コマンドを実行して、実行可能なインストーラーを作成できます：
```shell
yarn tbuild
```

### Linux

#### deb

Linuxホストで`yarn tbuild`コマンドを実行して、.debファイルを作成できます：
```shell
yarn tbuild
```

#### AppImage

Linuxホストで`yarn tbuild`コマンドを実行して、[AppImage](https://appimage.github.io/)を作成できます：
```shell
yarn tbuild
```

### macOS

#### dmg

macOSホストで`yarn tbuild`コマンドを実行して、macOSビルドを作成できます：
```shell
yarn tbuild
```

#### アーカイブ

macOSホストで`yarn tbuild`コマンドを実行して、macOSビルドを作成できます：
```shell
yarn tbuild
```

## クレジット

### Tauri

このプロジェクトは[Tauri](https://tauri.app/)を使用して、クロスプラットフォームアプリケーションを作成しています。

### ReactJS

このプロジェクトは[React](https://reactjs.org/)を使用してユーザーインターフェースを作成しています。

### テーマ

このアプリケーションで使用されているテーマは[MUI](https://mui.com/)です。

### 画像

アプリケーションアイコン（およびその派生）は[RemixIcon](https://remixicon.com/)によって提供されています。  
その他のすべての画像は[MUI](https://mui.com/material-ui/material-icons/)によって提供されました。

### 翻訳

- 中国語 (簡体字) - [王先生笔记](https://wxsnote.cn)
- イタリア語 - [bovirus](https://github.com/bovirus)
- 日本語 - [coolvitto](https://github.com/coolvitto)

## 概要

このライブラリはCodeDeadによって管理されています。以下のリンクから私たちの詳細を確認できます：
* [ウェブサイト](https://codedead.com)
* [Bluesky](https://bsky.app/profile/codedead.com)
* [Facebook](https://facebook.com/deadlinecodedead)

Copyright © 2025 CodeDead
