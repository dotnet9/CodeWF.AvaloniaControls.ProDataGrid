using System;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.Models;

public sealed class PerformanceItem(int index, int seed, string scenarioName)
{
    private static readonly string[] Statuses = ["运行", "待机", "换型", "点检", "告警"];
    private static readonly string[] ProductModels = ["AX-100", "AX-120", "BX-300", "CX-520", "DX-760"];
    private static readonly string[] Owners = ["张工", "李工", "王工", "赵工", "陈工", "刘工"];
    private static readonly string[] Shifts = ["早班", "中班", "夜班"];
    private static readonly DateTime SampleStartTime = new(2026, 5, 15, 8, 0, 0);

    private int RowNo => index + 1;

    private int LineNo => (index + seed) % 12 + 1;

    private int StationNo => index % 40 + 1;

    private int NodeNo => (index + seed) % 24 + 1;

    private int Plan => 800 + (index * 17 + seed) % 4200;

    private int Completed => Math.Min(Plan, 600 + (index * 31 + seed) % 4500);

    public int Id => RowNo;

    public string WorkOrder => $"WO-{LineNo:00}-{RowNo:000000}";

    public string LineName => $"总装 {LineNo:00} 线";

    public string StationName => $"工位 {StationNo:00}";

    public string DeviceCode => $"DEV-{LineNo:00}-{StationNo:00}";

    public string Name => $"采集终端 {StationNo:00}";

    public string BatchNo => $"B{index / 500 + 1:0000}";

    public string ProductModel => ProductModels[(index + seed) % ProductModels.Length];

    public string Status => Enabled ? Statuses[(index + seed) % Statuses.Length] : "停用";

    public bool Enabled => (index + seed) % 7 != 0;

    public string Shift => Shifts[(index / 300 + seed) % Shifts.Length];

    public string Owner => Owners[(index + seed) % Owners.Length];

    public int PlanQuantity => Plan;

    public int CompletedQuantity => Completed;

    public double YieldRate => Math.Round(95.5 + (index + seed) % 450 / 100.0, 2);

    public double Temperature => Math.Round(22 + (index + seed) % 180 / 10.0, 1);

    public double Pressure => Math.Round(0.45 + (index + seed) % 90 / 100.0, 2);

    public int RunMinutes => (index * 7 + seed) % 1440;

    public string LastSampleTime => SampleStartTime.AddSeconds(index % 86400).ToString("MM-dd HH:mm:ss");

    public string Description => $"{scenarioName} 第 {RowNo:N0} 行虚拟数据";
}
