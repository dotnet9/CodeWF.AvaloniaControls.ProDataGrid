using Avalonia.Controls;
using Avalonia.Data;
using CodeWF.AvaloniaControls.ProDataGrid;
using System.Diagnostics.CodeAnalysis;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.Views;

internal static class DataGridColumnInitializer
{
    public static void EnsureProcessColumns(DataGrid dataGrid)
    {
        if (dataGrid.Columns.Count > 0)
        {
            return;
        }

        AddTextColumn(dataGrid, "序号", 90, "Id");
        AddTextColumn(dataGrid, "工单号", 150, "WorkOrder");
        AddTextColumn(dataGrid, "产线", 110, "LineName");
        AddTextColumn(dataGrid, "工位", 100, "StationName");
        AddTextColumn(dataGrid, "设备编码", 130, "DeviceCode");
        AddTextColumn(dataGrid, "设备名称", 150, "Name");
        AddTextColumn(dataGrid, "批次号", 120, "BatchNo");
        AddTextColumn(dataGrid, "产品型号", 110, "ProductModel");
        AddTextColumn(dataGrid, "状态", 90, "Status");
        AddTextColumn(dataGrid, "启用", 80, "Enabled");
        AddTextColumn(dataGrid, "班次", 90, "Shift");
        AddTextColumn(dataGrid, "负责人", 90, "Owner");
        AddTextColumn(dataGrid, "计划数量", 110, "PlanQuantity");
        AddTextColumn(dataGrid, "完成数量", 110, "CompletedQuantity");
        AddTextColumn(dataGrid, "良率(%)", 100, "YieldRate");
        AddTextColumn(dataGrid, "温度(℃)", 100, "Temperature");
        AddTextColumn(dataGrid, "压力(MPa)", 110, "Pressure");
        AddTextColumn(dataGrid, "运行分钟", 110, "RunMinutes");
        AddTextColumn(dataGrid, "最近采集", 140, "LastSampleTime");
        AddTextColumn(dataGrid, "备注", 260, "Description");
    }

    public static void ApplyDefaultBehavior(DataGrid dataGrid)
    {
        dataGrid.ApplyPerformancePreset();
        dataGrid.EnableSmartTooltips(1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 18, 19);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The performance demo intentionally uses reflection binding to build columns dynamically.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The performance demo intentionally uses reflection binding to build columns dynamically.")]
    private static void AddTextColumn(DataGrid dataGrid, string header, double width, string path)
    {
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = header,
            Width = new DataGridLength(width),
            CanUserSort = false,
            SortMemberPath = path,
            Binding = new Binding(path)
        });
    }
}
