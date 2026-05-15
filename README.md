# CodeWF.AvaloniaControls.ProDataGrid

| Name | NuGet | Download |
|------|-------|----------|
| CodeWF.AvaloniaControls.ProDataGrid | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) |

Avalonia 12 extensions for the MIT-licensed `ProDataGrid` package, including tri-state sorting helpers, performance presets, and runnable samples.

English | [简体中文](README.zh-CN.md)

## Install

```shell
Install-Package CodeWF.AvaloniaControls.ProDataGrid
```

## Repository Layout

- `src/CodeWF.AvaloniaControls.ProDataGrid`: reusable ProDataGrid extension library
- `src/CodeWF.AvaloniaControls.ProDataGrid.Themes`: minimal ProDataGrid template style adjustments
- `src/CodeWF.AvaloniaControls.ProDataGridDemo`: combined functional and performance samples
- `CodeWF.AvaloniaControls.ProDataGrid.slnx`: solution view for the ProDataGrid library and samples

## Scripts

- `pack.bat`: restore, build, and pack `CodeWF.AvaloniaControls.ProDataGrid` into `artifacts/packages`
- `publish_all.bat`: publish the ProDataGrid demo application into `publish/`
- `publishbase.bat`: shared publish helper used by the sample publish script

## Notes

- This repository intentionally depends on the open-source `ProDataGrid` package line.
- The demo uses a single MVVM sample application with left-side tabs for each scenario.
