using System.Collections.Generic;
using CodeWF.AvaloniaControls.ProDataGridDemo.Models;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;

public class LargeDataGridViewModel : ViewModelBase
{
    private readonly int _rowCount;
    private readonly int _seed;
    private IReadOnlyList<PerformanceItem>? _items;

    public LargeDataGridViewModel(int rowCount, int seed, string headerTitle, string headerDescription)
    {
        _rowCount = rowCount;
        _seed = seed;
        HeaderTitle = headerTitle;
        HeaderDescription = headerDescription;
        Summary = $"共 {rowCount:N0} 行 / 20 列，按索引懒生成数据，用于观察大表滚动与列宽调整。";
    }

    public string HeaderTitle { get; }

    public string HeaderDescription { get; }

    public IReadOnlyList<PerformanceItem> Items => _items ??= PerformanceDataFactory.CreateRows(_rowCount, _seed, HeaderTitle);

    public string Summary { get; }
}
