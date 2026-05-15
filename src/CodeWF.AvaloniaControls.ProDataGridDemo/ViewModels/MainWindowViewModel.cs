using CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels.Pages;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        LargeDataGrid120 = new LargeDataGridViewModel(
            120000,
            23,
            "十万行切换页签",
            "120,000 行常驻数据，观察页签来回切换时是否依然顺畅。");

        LargeDataGrid180 = new LargeDataGridViewModel(
            180000,
            29,
            "二十万行切换页签",
            "180,000 行更高压力场景，放大滚动和重绘上的潜在卡顿。");
    }

    public SortingViewModel Sorting { get; } = new();

    public CrossRowsAndColumnsViewModel GroupedHeaders { get; } = new();

    public DynamicColumnsViewModel DynamicColumns { get; } = new();

    public DataGridDemoViewModel BasicPerformance { get; } = new();

    public LargeDataGridViewModel LargeDataGrid120 { get; }

    public LargeDataGridViewModel LargeDataGrid180 { get; }

    public DocumentWorkspaceViewModel DocumentWorkspace { get; } = new();
}
