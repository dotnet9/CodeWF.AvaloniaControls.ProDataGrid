using System.Collections.Generic;
using CodeWF.AvaloniaControls.ProDataGridDemo.Models;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;

public class LargeDataGridViewModel : ViewModelBase
{
    private readonly int _rowCount;
    private readonly int _seed;
    private IReadOnlyList<ProcessItem>? _items;

    public LargeDataGridViewModel(int rowCount, int seed, string headerTitle, string headerDescription)
    {
        _rowCount = rowCount;
        _seed = seed;
        HeaderTitle = headerTitle;
        HeaderDescription = headerDescription;
        Summary = $"共 {rowCount:N0} 行，直接保活在页签中，用于观察切换、滚动和排序时是否出现明显卡顿。";
    }

    public string HeaderTitle { get; }

    public string HeaderDescription { get; }

    public IReadOnlyList<ProcessItem> Items => _items ??= PerformanceDataFactory.CreateRows(_rowCount, _seed, HeaderTitle);

    public string Summary { get; }
}
