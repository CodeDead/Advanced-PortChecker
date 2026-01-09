# Advanced PortChecker

![Advanced PortChecker](https://i.imgur.com/vdt1sXZ.png)

[English](README.md) | [简体中文](README.zh-CN.md) | [Italiano](README.it.md) | [日本語](README.ja.md)

![GitHub](https://img.shields.io/badge/language-JavaScript+Rust-green)
![GitHub](https://img.shields.io/github/license/CodeDead/Advanced-PortChecker)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/CodeDead/Advanced-PortChecker)
[![Test](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/test.yml/badge.svg)](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/test.yml)
[![Release](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/release.yml/badge.svg)](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/release.yml)

Advanced PortChecker 是一款免费且开源的应用程序，可以帮助您检查特定主机上的端口是开放还是关闭。您可以一次检查多个端口，并将结果导出为纯文本、CSV 或 JSON 格式！

## 构建

Advanced PortChecker 使用 tauri 来构建桌面应用程序。您可以在[此处](https://tauri.app/v1/guides/)找到有关 Tauri 的更多信息。

有关使用 `vite` 进行构建的更多信息，请阅读[此处](https://vitejs.dev/guide/build.html)的 `Vite` 文档。

### 开发

您可以通过运行 `yarn tdev` 命令来启动开发预览：
```shell
yarn tdev
```

### Windows

#### 安装程序

您可以通过在 Windows 主机上运行 `yarn tbuild` 命令来创建可执行安装程序：
```shell
yarn tbuild
```

### Linux

#### deb

您可以通过在 Linux 主机上运行 `yarn tbuild` 命令来创建一个 .deb 文件：
```shell
yarn tbuild
```

#### AppImage

您可以通过在 Linux 主机上执行 `yarn tbuild` 命令来创建一个 [AppImage](https://appimage.github.io/)：
```shell
yarn tbuild
```

### macOS

#### dmg

您可以通过在 macOS 主机上运行 `yarn tbuild` 命令来创建 macOS 构建版本：
```shell
yarn tbuild
```

#### 归档 (Archive)

您可以通过在 macOS 主机上运行 `yarn tbuild` 命令来创建 macOS 构建版本：
```shell
yarn tbuild
```

## 致谢

### Tauri

本项目使用 [Tauri](https://tauri.app/) 来创建跨平台应用程序。

### ReactJS

本项目使用 [React](https://reactjs.org/) 来创建用户界面。

### 主题 (Theme)

本应用程序中使用的主题是 [MUI](https://mui.com/)。

### 图像 (Images)

应用程序图标（及其衍生产品）由 [RemixIcon](https://remixicon.com/) 提供。  
所有其他图像均由 [MUI](https://mui.com/material-ui/material-icons/) 提供。

### 翻译 (Translations)

- 简体中文 - [王先生笔记](https://wxsnote.cn)
- 意大利语 - [bovirus](https://github.com/bovirus)
- 日语 - [coolvitto](https://github.com/coolvitto)

## 关于

此库由 CodeDead 维护。您可以通过以下链接找到关于我们的更多信息：
* [网站](https://codedead.com)
* [Bluesky](https://bsky.app/profile/codedead.com)
* [Facebook](https://facebook.com/deadlinecodedead)

Copyright © 2025 CodeDead
