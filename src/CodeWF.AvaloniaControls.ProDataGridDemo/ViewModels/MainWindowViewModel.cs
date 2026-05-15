using CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels.Pages;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public SortingViewModel Sorting { get; } = new();

    public CrossRowsAndColumnsViewModel GroupedHeaders { get; } = new();

    public DynamicColumnsViewModel DynamicColumns { get; } = new();

    public LargeDataGridViewModel MillionRows { get; } = new(
        1_000_000,
        53,
        "百万行大表性能",
        "1,000,000 行、20 列的生产监控明细，重点观察虚拟化滚动、列宽调整和横向浏览。");
}
