namespace CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;

public class DocumentWorkspaceViewModel : ViewModelBase
{
    public LargeDataGridViewModel ProductionOverview { get; } = new(
        90000,
        31,
        "产线总览文档",
        "90,000 行实时监控数据，模拟总览页签常驻。");

    public LargeDataGridViewModel AlarmTracking { get; } = new(
        110000,
        37,
        "告警追踪文档",
        "110,000 行告警记录，模拟值守人员在多文档之间快速切换。");

    public LargeDataGridViewModel ProcessParameters { get; } = new(
        140000,
        41,
        "工艺参数文档",
        "140,000 行工艺参数，模拟频繁查看和横向滚动。");

    public LargeDataGridViewModel ExecutionLog { get; } = new(
        160000,
        47,
        "执行日志文档",
        "160,000 行执行日志，专门压测大数据量文档回切场景。");
}
