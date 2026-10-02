<div align="center">

# 慕寒手环解锁 MuHan Band Unlock

**本工具是用于解锁忘记密码的小米手环的。**

[![Build](https://github.com/bilibiliHaoziyao/muhanbandunlock/actions/workflows/build.yml/badge.svg)](../../actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

</div>

---

## ⚠️ 免责声明（使用前请阅读）

- **本工具是用于解锁忘记密码的小米手环的。**
- 本工具仅供个人解锁**自己拥有的设备**使用，请勿用于他人设备或任何非法用途。
- 本工具为第三方开源工具，**与小米公司无任何关联**；小米、Mi Band 为其各自所有者的商标。
- 解锁操作会**重置手环密码**，可能清除手环上的数据；使用本工具所产生的任何后果由使用者自行承担。
- 继续使用即表示你已阅读并同意以上条款。

## 功能

- 🎨 现代化深色 UI，完整支持触屏操作（大按钮、即时反馈）
- 🔐 一键生成 10 位解锁码，支持复制到剪贴板
- ⚙️ 支持新旧两种算法（S5 / 10 Pro 及更新机型请开启「新版算法」）
- 📦 兼容 x86 / x64 / arm64，最低支持 **Windows 10 1507（10.0.10240.0）**

## 使用方法

1. 从 [Releases](../../releases) 下载 `MuHanBandUnlock-Sideload-Installer.zip` 并解压
2. 双击 `MuHanBandUnlock.cer`，将证书安装到 **受信任的根证书颁发机构**
3. 在解压目录运行：

   ```powershell
   Set-ExecutionPolicy -Scope Process Bypass -Force
   .\Add-AppDevPackage.ps1
   ```

4. 打开应用，输入手环的 **MAC 地址** 与 **序列号 SN**（可在小米运动 App 或包装盒上找到）
5. S5 / 10 Pro 及更新机型打开「新版算法」开关，点击「生成解锁码」
6. 在手环密码输入界面依次输入生成的 10 位数字即可解锁

## 从源码构建

需要 Visual Studio 2022（含 **通用 Windows 平台开发** 工作负载）与 Windows 10 SDK 10.0.19041.0：

```powershell
nuget restore MuHanBandUnlock.sln
msbuild MuHanBandUnlock.sln /p:Configuration=Release /p:Platform=x64
```

或直接用 Visual Studio 打开 `MuHanBandUnlock.sln` 部署运行。CI 配置见 [.github/workflows/build.yml](.github/workflows/build.yml)。

## 算法说明

解锁算法移植自 [leset0ng/ab-unlockcode](https://github.com/leset0ng/ab-unlockcode)（MIT License）：

1. 归一化 MAC（去除 `：` `:` `-` 空格 `.` 并转大写）与 SN（去空白转大写）
2. 拼接为 `SN + MAC + "XIAOMI"`（新版算法）或 `MAC + SN + "XIAOMI"`（旧版算法）
3. 计算 SHA-256，取前 10 字节对 `0xA` 取模，得到 10 位解锁码

本地已通过 7 组测试用例与参考实现（Rust 版）逐位比对验证一致。

## 致谢

- 算法来源：[leset0ng/ab-unlockcode](https://github.com/leset0ng/ab-unlockcode)

## License

[MIT](LICENSE)
