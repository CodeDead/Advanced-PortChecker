# 高级端口检查器

![Advanced PortChecker](https://i.imgur.com/vdt1sXZ.png)

![GitHub](https://img.shields.io/badge/language-JavaScript+Rust-green)
![GitHub](https://img.shields.io/github/license/CodeDead/Advanced-PortChecker)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/CodeDead/Advanced-PortChecker)
[![测试](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/test.yml/badge.svg)](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/test.yml)
[![发布](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/release.yml/badge.svg)](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/release.yml)

高级端口检查器是一款免费开源应用程序，可帮助您检查特定主机上的端口是开放还是关闭状态。您可以一次性检查多个端口，并以纯文本、CSV或JSON格式导出结果！

## 构建

高级端口检查器使用Tauri构建桌面应用程序。您可以在[这里](https://tauri.app/v1/guides/)找到更多关于Tauri的信息。

有关使用`vite`构建的更多信息，请阅读[Vite文档](https://vitejs.dev/guide/build.html)。

### 开发

您可以运行`yarn tdev`命令启动开发预览：
```shell
yarn tdev
```

### Windows

#### 安装程序

您可以在Windows主机上运行`yarn tbuild`命令创建可执行安装程序：
```shell
yarn tbuild
```

### Linux

#### deb包

您可以在Linux主机上运行`yarn tbuild`命令创建.deb文件：
```shell
yarn tbuild
```

#### AppImage

您可以在Linux主机上执行`yarn tbuild`命令创建[AppImage](https://appimage.github.io/)：
```shell
yarn tbuild
```

### macOS

#### dmg镜像

您可以在macOS主机上运行`yarn tbuild`命令创建macOS构建：
```shell
yarn tbuild
```

#### 归档文件

您可以在macOS主机上运行`yarn tbuild`命令创建macOS构建：
```shell
yarn tbuild
```

## 致谢

### Tauri

本项目使用[Tauri](https://tauri.app/)创建跨平台应用程序。

### ReactJS

本项目使用[React](https://reactjs.org/)创建用户界面。

### 主题

本应用程序使用的主题来自[MUI](https://mui.com/)。

### 图像

应用程序图标（及其衍生作品）由[RemixIcon](https://remixicon.com/)提供。  
所有其他图像由[MUI](https://mui.com/material-ui/material-icons/)提供。

### 翻译

- 中文 (简体) - [王先生笔记](https://wxsnote.cn)
- 意大利语 - [bovirus](https://github.com/bovirus)
- 日语 - [coolvitto](https://github.com/coolvitto)

## 关于

本库由 CodeDead 维护。您可以通过以下链接了解更多关于我们的信息：
* [官网](https://codedead.com)
* [Bluesky](https://bsky.app/profile/codedead.com)
* [Facebook](https://facebook.com/deadlinecodedead)

版权所有 © 2025 CodeDead
