# Advanced PortChecker

![Advanced PortChecker](https://i.imgur.com/vdt1sXZ.png)

![GitHub](https://img.shields.io/badge/language-JavaScript+Rust-green)
![GitHub](https://img.shields.io/github/license/CodeDead/Advanced-PortChecker)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/CodeDead/Advanced-PortChecker)
[![Test](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/test.yml/badge.svg)](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/test.yml)
[![Release](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/release.yml/badge.svg)](https://github.com/CodeDead/Advanced-PortChecker/actions/workflows/release.yml)

Advanced PortChecker is a free and open-source application that can help you check if ports are open or closed on a certain host. You can check multiple ports at once and export the results in plain text, CSV or JSON format!

[中文]([https://tauri.app/v1/guides/](https://github.com/twsh0305/Advanced-PortChecker/blob/master/README_CN.md))

## Building

Advanced PortChecker uses tauri to build the desktop application. You can find more information about Tauri [here](https://tauri.app/v1/guides/).

For more information about building using `vite`, please read the `Vite` documentation [here](https://vitejs.dev/guide/build.html).

### Development

You can start a development preview by running the `yarn tdev` command:
```shell
yarn tdev
```

### Windows

#### Installer

You can create an executable installer by running the `yarn tbuild` command on a Windows host:
```shell
yarn tbuild
```

### Linux

#### deb

You can create a .deb file, by running the `yarn tbuild` command on a Linux host:
```shell
yarn tbuild
```

#### AppImage

You can create an [AppImage](https://appimage.github.io/) by executing the `yarn tbuild` command on a Linux host:
```shell
yarn tbuild
```

### macOS

#### dmg

You can create a macOS build by running the `yarn tbuild` command on a macOS host:
```shell
yarn tbuild
```

#### Archive

You can create a macOS build by running the `yarn tbuild` command on a macOS host:
```shell
yarn tbuild
```

## Credits

### Tauri

This project uses [Tauri](https://tauri.app/) to create a cross-platform application.

### ReactJS

This project uses [React](https://reactjs.org/) to create the user interface.

### Theme

The theme used in this application is [MUI](https://mui.com/).

### Images

The application icon (and derivatives) are provided by [RemixIcon](https://remixicon.com/).  
All other images were provided by [MUI](https://mui.com/material-ui/material-icons/).

### Translations

- Chinese (Simplified) - [王先生笔记](https://wxsnote.cn)
- Italian - [bovirus](https://github.com/bovirus)
- Japanese - [coolvitto](https://github.com/coolvitto)

## About

This library is maintained by CodeDead. You can find more about us using the following links:
* [Website](https://codedead.com)
* [Bluesky](https://bsky.app/profile/codedead.com)
* [Facebook](https://facebook.com/deadlinecodedead)

Copyright © 2025 CodeDead
