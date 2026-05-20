# Changelog

[简体中文](CHANGELOG.zh-CN.md) | English

## V12.0.3.2（2026-05-20）

- 🔨[优化]-Added `Avalonia.Themes.Fluent` as the open ProDataGrid template baseline so demo column headers render after removing `Semi.Avalonia.ProDataGrid`.
- 🔨[优化]-Kept the demo on the open `Semi.Avalonia` core theme while loading `CodeWFProDataGridTheme` for ProDataGrid-specific Fluent DataGrid resources and column-header adjustments.

## V12.0.3.1（2026-05-15）

- 😄[新增]-Added `CodeWF.AvaloniaControls.ProDataGridDemo` as the single combined demo for functional and performance scenarios.
- 🔨[优化]-Moved the demo navigation to left-side tabs and removed the large showcase headers.
- 🔨[优化]-Tightened grouped-header and dynamic-column demo visuals, including grouped header lines, cell content spacing, hover fill, separator tone, and the dynamic diagonal header.
- 😄[新增]-Added a minimal `CodeWF.AvaloniaControls.ProDataGrid.Themes` package for open-source ProDataGrid style adjustments.
- 🔨[优化]-Removed the low-value basic interaction and document workspace demo tabs from the combined demo.
- 🔨[优化]-Consolidated the large data scenarios into a single 1,000,000-row by 20-column performance table with lightweight generated rows.
- 🔨[优化]-Limited the dynamic-column demo to 20 visible rows and no more than 10 total columns, with clearer device metric labels.
- 🔨[优化]-Refined grouped-header captions, sample data, and horizontal header lines so they align with the active column width.
- 😄[新增]-Added a demo-wide scrollbar expansion toggle and padded generated/grouped column headers for better readability.
- 🔨[优化]-Made the scrollbar expansion toggle apply directly to live scroll viewers, removed redundant per-page demo title strips, and widened grouped-header captions to avoid clipping near resize grips.
- 🔨[优化]-Trimmed package dependencies so the core and theme libraries depend on `ProDataGrid` directly without forcing Semi.Avalonia packages.

## V12.0.2.1（2026-05-08）

- 😄[新增]-Migrated `CodeWF.AvaloniaControls.ProDataGrid` and its ProDataGrid sample applications into this standalone repository.
- 😄[新增]-Added ProDataGrid-only solution, central package versions, packing script, and sample publish script.
