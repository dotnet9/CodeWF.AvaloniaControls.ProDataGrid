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
- `src/CodeWF.AvaloniaControls.ProDataGridShowcase`: functional ProDataGrid showcase
- `src/CodeWF.AvaloniaControls.ProDataGridPerformanceDemo`: large-data, tab-switching, and document-switching performance sample
- `CodeWF.AvaloniaControls.ProDataGrid.slnx`: solution view for the ProDataGrid library and samples

## Scripts

- `pack.bat`: restore, build, and pack `CodeWF.AvaloniaControls.ProDataGrid` into `artifacts/packages`
- `publish_all.bat`: publish all ProDataGrid sample applications into `publish/`
- `publishbase.bat`: shared publish helper used by the sample publish script

## Notes

- This repository intentionally depends on the open-source `ProDataGrid` package line.
- `Prism.DryIoc.Avalonia` is pinned to `8.1.97.11073` because the `9.x` line is commercial.
