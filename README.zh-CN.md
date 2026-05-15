# CodeWF.AvaloniaControls.ProDataGrid

| 名称 | NuGet | 下载量 |
|------|-------|--------|
| CodeWF.AvaloniaControls.ProDataGrid | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) |

这是 `CodeWF.AvaloniaControls.ProDataGrid` 的独立仓库，用于维护 Avalonia 12 下基于 MIT 协议 `ProDataGrid` 的扩展类库与示例工程。

[English](README.md) | 简体中文

## 安装

```powershell
Install-Package CodeWF.AvaloniaControls.ProDataGrid
```

## 仓库结构

- `src/CodeWF.AvaloniaControls.ProDataGrid`：可复用的 ProDataGrid 扩展类库
- `src/CodeWF.AvaloniaControls.ProDataGrid.Themes`：面向 ProDataGrid 模板的最小样式补丁
- `src/CodeWF.AvaloniaControls.ProDataGridDemo`：合并后的功能与性能示例工程
- `CodeWF.AvaloniaControls.ProDataGrid.slnx`：ProDataGrid 类库和示例的解决方案视图

## 脚本

- `pack.bat`：还原、构建并打包 `CodeWF.AvaloniaControls.ProDataGrid` 到 `artifacts/packages`
- `publish_all.bat`：发布 ProDataGrid 示例工程到 `publish/`
- `publishbase.bat`：示例发布脚本共用的辅助脚本

## 说明

- 当前仓库明确使用开源 `ProDataGrid` 包线。
- 示例工程使用单一 MVVM 应用，并通过左侧页签切换每个演示场景。
