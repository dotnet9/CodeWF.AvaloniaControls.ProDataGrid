# CodeWF.AvaloniaControls.ProDataGrid

| 名称 | NuGet | 下载量 |
|------|-------|--------|
| CodeWF.AvaloniaControls.ProDataGrid | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) |

这是 `CodeWF.AvaloniaControls.ProDataGrid` 的独立仓库，用于维护 Avalonia 12 下基于 MIT 协议 `ProDataGrid` 的扩展类库与示例工程。

## 仓库规范

- 当前版本：`12.1.0.2`，版本号统一维护在根目录 `Directory.Build.props` 的 `<Version>` 节点。
- NuGet 包项目统一支持 `net8.0;net10.0`；Demo、App、测试与内部应用项目统一使用 `net11.0` / `net11.0-windows`。
- 根目录 `logo.svg`、`logo.png`、`logo.ico` 是唯一图标源，子工程只通过 MSBuild `Link` 引用，不维护图标副本。
- 运行时帮助、Markdown 示例、内置备忘录、设计说明等业务文档按功能保留；仓库级入口文档使用根目录 `README.md` 和 `UpdateLog.md`。

## 安装

```powershell
Install-Package CodeWF.AvaloniaControls.ProDataGrid
Install-Package CodeWF.AvaloniaControls.ProDataGrid.Themes
```

## 通用表格增强

```xml
<Style Selector="DataGrid">
  <Setter Property="(codewf:DataGridEnhancement.UseDefaultEnhancements)" Value="True" />
</Style>
```

`UseDefaultEnhancements` 会统一启用三态排序、可排序绑定列的自然排序和智能 ToolTip。代码侧也可以直接调用 `dataGrid.ApplyDefaultEnhancements()` 或 `dataGrid.AddNaturalSorting()`。

## 主题配置

如果应用使用 Semi.Avalonia，可以继续保留开源 `Semi.Avalonia` 主体主题，并在 `FluentTheme` 后加入 CodeWF ProDataGrid 主题：

```xml
<Application.Styles>
  <FluentTheme />
  <semi:SemiTheme Locale="zh-CN" />
  <codewf:CodeWFProDataGridTheme />
</Application.Styles>
```

`CodeWFProDataGridTheme` 会加载 MIT 协议的 `ProDataGrid` Fluent DataGrid 模板资源，然后叠加 CodeWF 的列头样式调整。它不引用 `Semi.Avalonia.ProDataGrid`。

## 仓库结构

- `src/CodeWF.AvaloniaControls.ProDataGrid`：可复用的 ProDataGrid 扩展类库
- `src/CodeWF.AvaloniaControls.ProDataGrid.Themes`：ProDataGrid Fluent 模板资源与 CodeWF 最小样式补丁
- `src/CodeWF.AvaloniaControls.ProDataGridDemo`：合并后的功能与性能示例工程
- `CodeWF.AvaloniaControls.ProDataGrid.slnx`：ProDataGrid 类库和示例的解决方案视图

## 脚本

- `pack.bat`：还原、构建并打包 `CodeWF.AvaloniaControls.ProDataGrid` 到 `artifacts/packages`
- `publish_all.bat`：发布 ProDataGrid 示例工程到 `publish/`
- `publishbase.bat`：示例发布脚本共用的辅助脚本

## 说明

- 当前仓库明确使用开源 `ProDataGrid` 包线。
- 示例工程保留开源 `Semi.Avalonia` 主体主题，同时使用 `Avalonia.Themes.Fluent` 作为 ProDataGrid 模板基线。
- 示例工程使用单一 MVVM 应用，并通过左侧页签切换每个演示场景。

## 第三方开源组件审计

检查时间：2026-05-20。检查范围包括 NuGet 元数据、恢复后的 `project.assets.json`、NuGet.org 信息以及上游源码/许可证链接。优先接受 MIT / Apache-2.0 / BSD。

本次整改：

- 移除 `Semi.Avalonia.ProDataGrid`；该包只提供 Semi 主题，未找到公开源码仓库。
- 新增 `CodeWF.AvaloniaControls.ProDataGrid.Themes`，在本仓库中维护可审计的 ProDataGrid 主题资源。
- 新增 MIT 协议的 `Avalonia.Themes.Fluent` 基线，使 ProDataGrid 列头保留模板样式，同时示例仍使用开源 `Semi.Avalonia` 主体主题。
- 示例工程移除 `AvaloniaUI.DiagnosticsSupport`，因为该包未公开明确的开源许可证和源码仓库。

| 包 | 协议 | 源码/项目地址 | 结论 |
| --- | --- | --- | --- |
| `Avalonia` / `Avalonia.Desktop` / `Avalonia.Fonts.Inter` / `Avalonia.Themes.Fluent` | MIT | https://github.com/AvaloniaUI/Avalonia | 通过 |
| `CodeWF.AvaloniaControls.ProDataGrid.Themes` | MIT | https://github.com/dotnet9/CodeWF.AvaloniaControls.ProDataGrid | 自研开源包 |
| `ProDataGrid` | MIT | https://github.com/wieslawsoltes/ProDataGrid | 通过 |
| `ReactiveUI.Avalonia` | MIT | https://github.com/reactiveui/reactiveui | 通过 |
| `Semi.Avalonia` | MIT | https://github.com/irihitech/Semi.Avalonia | 通过，仅使用开源主体包 |
| `System.Drawing.Common` | MIT | https://github.com/dotnet/dotnet | 通过，固定到 `10.0.9` |
| `VC-LTL` | EPL-2.0 | https://github.com/Chuyu-Team/VC-LTL5 | 源码开放，按“源码可追溯的非优先协议”通过 |
| `YY-Thunks` | MIT | https://github.com/Chuyu-Team/YY-Thunks | 通过 |

从恢复资产穿透检查的传递依赖分组：

| 包分组 | 协议 | 源码/项目地址 | 结论 |
| --- | --- | --- | --- |
| `Avalonia.*`（不含 ANGLE 原生包） | MIT | https://github.com/AvaloniaUI/Avalonia | 通过 |
| `Avalonia.Angle.Windows.Natives` | BSD 风格许可证文件 | https://github.com/AvaloniaUI/angle | 通过 |
| `Avalonia.BuildServices` | MIT | https://github.com/AvaloniaUI/Avalonia.BuildServices | 通过 |
| `DynamicData` | MIT | https://github.com/reactiveui/DynamicData | 通过 |
| `HarfBuzzSharp*` / `SkiaSharp*` | MIT | https://github.com/mono/SkiaSharp | 通过 |
| `Irihi.Avalonia.Shared` | MIT | https://github.com/irihitech/Irihi.Avalonia.Shared | 通过 |
| `MicroCom.Runtime` | MIT | https://github.com/kekekeks/MicroCom | 通过 |
| `Microsoft.NET.ILLink.Tasks` / `Microsoft.Win32.SystemEvents` | MIT | https://github.com/dotnet/dotnet | 通过 |
| `ProDataGrid.FormulaEngine*` | MIT | https://github.com/wieslawsoltes/ProDataGrid | 通过 |
| `ReactiveUI` | MIT | https://github.com/reactiveui/reactiveui | 通过 |
| `Splat*` | MIT | https://github.com/reactiveui/splat | 通过 |
| `System.Reactive` | MIT | https://github.com/dotnet/reactive | 通过 |
| `Tmds.DBus.Protocol` | MIT | https://github.com/tmds/Tmds.DBus | 通过 |

有效恢复资产中不再包含 `Semi.Avalonia.ProDataGrid`。
## 包版本维护约定

XML 文件统一使用两个空格缩进。`Directory.Packages.props` 统一承载 NuGet 中央包管理开关和包版本变量，包括 `AvaloniaVersion` 等共享版本属性；`Directory.Build.props` 仅保留项目构建、编译选项和 NuGet 元数据。仓库如引用 `VC-LTL`、`YY-Thunks`，这两个兼容旧版操作系统的特殊包应优先使用最新稳定版；如稳定版暂未覆盖目标场景，再使用最新预览版。
