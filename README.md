# CodeWF.AvaloniaControls.ProDataGrid

| Name | NuGet | Download |
|------|-------|----------|
| CodeWF.AvaloniaControls.ProDataGrid | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) |

Avalonia 12 extensions for the MIT-licensed `ProDataGrid` package, including tri-state sorting helpers, performance presets, and runnable samples.

English | [简体中文](README.zh-CN.md)

## Install

```shell
Install-Package CodeWF.AvaloniaControls.ProDataGrid
Install-Package CodeWF.AvaloniaControls.ProDataGrid.Themes
```

## Theme Setup

When the app uses Semi.Avalonia, keep the open `Semi.Avalonia` core theme and add the CodeWF ProDataGrid theme after `FluentTheme`:

```xml
<Application.Styles>
  <FluentTheme />
  <semi:SemiTheme Locale="zh-CN" />
  <codewf:CodeWFProDataGridTheme />
</Application.Styles>
```

`CodeWFProDataGridTheme` loads the MIT `ProDataGrid` Fluent DataGrid template resources and then applies the CodeWF column-header adjustments. It does not reference `Semi.Avalonia.ProDataGrid`.

## Repository Layout

- `src/CodeWF.AvaloniaControls.ProDataGrid`: reusable ProDataGrid extension library
- `src/CodeWF.AvaloniaControls.ProDataGrid.Themes`: ProDataGrid Fluent template resources plus minimal CodeWF style adjustments
- `src/CodeWF.AvaloniaControls.ProDataGridDemo`: combined functional and performance samples
- `CodeWF.AvaloniaControls.ProDataGrid.slnx`: solution view for the ProDataGrid library and samples

## Scripts

- `pack.bat`: restore, build, and pack `CodeWF.AvaloniaControls.ProDataGrid` into `artifacts/packages`
- `publish_all.bat`: publish the ProDataGrid demo application into `publish/`
- `publishbase.bat`: shared publish helper used by the sample publish script

## Notes

- This repository intentionally depends on the open-source `ProDataGrid` package line.
- The demo keeps the open `Semi.Avalonia` core package for the app look and uses `Avalonia.Themes.Fluent` as the ProDataGrid template baseline.
- The demo uses a single MVVM sample application with left-side tabs for each scenario.

## Third-Party Open Source Audit

Checked on 2026-05-20 with NuGet metadata, restored `project.assets.json`, and upstream source/license links. MIT / Apache-2.0 / BSD are preferred.

Remediation:

- Removed `Semi.Avalonia.ProDataGrid`; it only provides a Semi theme package and no public source repository was found.
- The package now uses the MIT-licensed `ProDataGrid` package directly and provides `CodeWFProDataGridTheme` in the CodeWF theme package.
- Added the MIT `Avalonia.Themes.Fluent` baseline so ProDataGrid column headers keep their template style while the demo still uses the open `Semi.Avalonia` core theme.
- Removed `AvaloniaUI.DiagnosticsSupport` from samples because the package does not publish a clear open-source license or source repository.

| Package | License | Source | Status |
| --- | --- | --- | --- |
| `Avalonia` / `Avalonia.Desktop` / `Avalonia.Fonts.Inter` / `Avalonia.Themes.Fluent` | MIT | https://github.com/AvaloniaUI/Avalonia | Approved |
| `CodeWF.AvaloniaControls.ProDataGrid.Themes` | MIT | https://github.com/dotnet9/CodeWF.AvaloniaControls.ProDataGrid | In-house open package |
| `ProDataGrid` | MIT | https://github.com/wieslawsoltes/ProDataGrid | Approved |
| `ReactiveUI.Avalonia` | MIT | https://github.com/reactiveui/reactiveui | Approved |
| `Semi.Avalonia` | MIT | https://github.com/irihitech/Semi.Avalonia | Approved, only the open core package is used by the demo |
| `System.Drawing.Common` | MIT | https://github.com/dotnet/dotnet | Approved, pinned to `10.0.8` |
| `VC-LTL` | EPL-2.0 | https://github.com/Chuyu-Team/VC-LTL5 | Source-open; approved under the source-traceable non-preferred license rule |
| `YY-Thunks` | MIT | https://github.com/Chuyu-Team/YY-Thunks | Approved |

Transitive dependency groups checked from the restored assets:

| Package group | License | Source | Status |
| --- | --- | --- | --- |
| `Avalonia.*` except the ANGLE native package | MIT | https://github.com/AvaloniaUI/Avalonia | Approved |
| `Avalonia.Angle.Windows.Natives` | BSD-style license file | https://github.com/AvaloniaUI/angle | Approved |
| `Avalonia.BuildServices` | MIT | https://github.com/AvaloniaUI/Avalonia.BuildServices | Approved |
| `DynamicData` | MIT | https://github.com/reactiveui/DynamicData | Approved |
| `HarfBuzzSharp*` / `SkiaSharp*` | MIT | https://github.com/mono/SkiaSharp | Approved |
| `Irihi.Avalonia.Shared` | MIT | https://github.com/irihitech/Irihi.Avalonia.Shared | Approved |
| `MicroCom.Runtime` | MIT | https://github.com/kekekeks/MicroCom | Approved |
| `Microsoft.NET.ILLink.Tasks` / `Microsoft.Win32.SystemEvents` | MIT | https://github.com/dotnet/dotnet | Approved |
| `ProDataGrid.FormulaEngine*` | MIT | https://github.com/wieslawsoltes/ProDataGrid | Approved |
| `ReactiveUI` | MIT | https://github.com/reactiveui/reactiveui | Approved |
| `Splat*` | MIT | https://github.com/reactiveui/splat | Approved |
| `System.Reactive` | MIT | https://github.com/dotnet/reactive | Approved |
| `Tmds.DBus.Protocol` | MIT | https://github.com/tmds/Tmds.DBus | Approved |

Active restore assets no longer contain `Semi.Avalonia.ProDataGrid`.
