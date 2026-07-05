using CodeWF.AvaloniaControls.ProDataGrid;
using CodeWF.AvaloniaControls.ProDataGridDemo.Models;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels.Pages;

public sealed class CommonDataGridViewModel : ReactiveObject
{
    private const int RecordCount = 200;
    private const string DefaultTargetName = "普通任务 1";
    private int? _highlightedId;
    private string? _highlightedName = DefaultTargetName;
    private int _targetFieldIndex = 1;
    private string _targetValueText = DefaultTargetName;
    private string _resultText = $"任务名称 {DefaultTargetName} 已设置为灰色并置顶";

    public CommonDataGridViewModel()
    {
        Records = new ObservableCollection<ProcessItem>(CreateCommonRecords(RecordCount));
    }

    public string Header { get; } = "通用示例";

    public ObservableCollection<ProcessItem> Records { get; }

    public int TargetFieldIndex
    {
        get => _targetFieldIndex;
        set => this.RaiseAndSetIfChanged(ref _targetFieldIndex, value);
    }

    public string TargetValueText
    {
        get => _targetValueText;
        set => this.RaiseAndSetIfChanged(ref _targetValueText, value);
    }

    public string ResultText
    {
        get => _resultText;
        private set => this.RaiseAndSetIfChanged(ref _resultText, value);
    }

    public bool SetTargetRowBackground()
    {
        var text = TargetValueText.Trim();
        if (string.IsNullOrWhiteSpace(text))
        {
            ResultText = "请输入匹配值";
            return false;
        }

        if (TargetFieldIndex == 0)
        {
            return SetTargetRowBackgroundById(text);
        }

        return SetTargetRowBackgroundByName(text);
    }

    private bool SetTargetRowBackgroundById(string text)
    {
        if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
        {
            ResultText = "请输入有效编号";
            return false;
        }

        if (Records.All(x => x.Id != id))
        {
            ResultText = $"未找到编号 {id}";
            return false;
        }

        _highlightedId = id;
        _highlightedName = null;
        BringHighlightedRecordToTop();
        ResultText = $"编号 {id} 已设置为灰色并置顶";
        return true;
    }

    private bool SetTargetRowBackgroundByName(string name)
    {
        if (Records.All(x => !string.Equals(x.Name, name, StringComparison.Ordinal)))
        {
            ResultText = $"未找到任务名称 {name}";
            return false;
        }

        _highlightedId = null;
        _highlightedName = name;
        BringHighlightedRecordToTop();
        ResultText = $"任务名称 {name} 已设置为灰色并置顶";
        return true;
    }

    public void ClearTargetRowBackground()
    {
        _highlightedId = null;
        _highlightedName = null;
        RestoreDefaultRecordOrder();
        ResultText = "已清除";
    }

    public bool ShouldHighlight(ProcessItem item)
    {
        return item.Id == _highlightedId ||
            (_highlightedName is not null &&
             string.Equals(item.Name, _highlightedName, StringComparison.Ordinal));
    }

    private void BringHighlightedRecordToTop()
    {
        var record = Records.FirstOrDefault(ShouldHighlight);
        if (record is null)
        {
            return;
        }

        var index = Records.IndexOf(record);
        if (index > 0)
        {
            Records.Move(index, 0);
        }
    }

    private void RestoreDefaultRecordOrder()
    {
        var orderedRecords = Records.OrderBy(x => x.Id).ToList();
        for (var targetIndex = 0; targetIndex < orderedRecords.Count; targetIndex++)
        {
            var currentIndex = Records.IndexOf(orderedRecords[targetIndex]);
            if (currentIndex != targetIndex)
            {
                Records.Move(currentIndex, targetIndex);
            }
        }
    }

    private static List<ProcessItem> CreateCommonRecords(int count)
    {
        var items = new List<ProcessItem>(count);
        var nameNumbers = CreateShuffledNumbers(count, 20260704);
        var random = new Random(20260705);

        for (var i = 1; i <= count; i++)
        {
            var sourceNode = i % 8 + 1;
            var lineNo = random.Next(1, 999);
            items.Add(new ProcessItem
            {
                Id = i,
                Name = $"普通任务 {nameNumbers[i - 1]}",
                Enabled = i % 6 != 0,
                SourceNode = sourceNode,
                Host = $"10.40.{sourceNode}.{i % 220 + 10}",
                ProgramPath = $@"D:\runtime\common\worker-{i % 8}.exe",
                WorkPath = $@"D:\runtime\common\workspace-{i % 12}",
                Params = i % 3 == 0 ? "--mode manual --trace" : "--mode auto --retry 1",
                AutoStart = i % 4 != 0,
                PreProcess = i % 5 == 0 ? $"check-line-{lineNo}" : string.Empty,
                PostProcess = i % 7 == 0 ? "archive-log" : string.Empty,
                Description = "用于验证 ProDataGrid 三态排序、智能提示和行状态设置。"
            });
        }

        return items;
    }

    private static List<int> CreateShuffledNumbers(int count, int seed)
    {
        var numbers = Enumerable.Range(1, count).ToList();
        var random = new Random(seed);

        for (var i = numbers.Count - 1; i > 0; i--)
        {
            var targetIndex = random.Next(i + 1);
            (numbers[i], numbers[targetIndex]) = (numbers[targetIndex], numbers[i]);
        }

        var firstValueIndex = numbers.IndexOf(1);
        if (firstValueIndex > 0)
        {
            (numbers[0], numbers[firstValueIndex]) = (numbers[firstValueIndex], numbers[0]);
        }

        return numbers;
    }
}

internal interface IProcessItemPinComparer
{
    Func<ProcessItem, bool>? ShouldPin { get; set; }
}

internal sealed class ProcessItemSortComparer<TValue> : IDataGridSortDirectionAwareComparer, IProcessItemPinComparer
{
    private readonly Func<ProcessItem, TValue?> _selector;

    public Func<ProcessItem, bool>? ShouldPin { get; set; }

    public ListSortDirection SortDirection { get; set; } = ListSortDirection.Ascending;

    public ProcessItemSortComparer(Func<ProcessItem, TValue?> selector)
    {
        _selector = selector;
    }

    public int Compare(object? x, object? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (x is not ProcessItem xItem)
        {
            return y is ProcessItem ? -1 : 0;
        }

        if (y is not ProcessItem yItem)
        {
            return 1;
        }

        var pinComparison = GetPinRank(xItem).CompareTo(GetPinRank(yItem));
        if (pinComparison != 0)
        {
            return SortDirection == ListSortDirection.Descending ? -pinComparison : pinComparison;
        }

        return CompareValues(_selector(xItem), _selector(yItem));
    }

    private int GetPinRank(ProcessItem item)
    {
        return ShouldPin?.Invoke(item) == true ? 0 : 1;
    }

    private static int CompareValues(TValue? x, TValue? y)
    {
        if (x is string xText && y is string yText)
        {
            return NaturalStringComparer.Compare(xText, yText);
        }

        return Comparer<TValue>.Default.Compare(x, y);
    }

    private static class NaturalStringComparer
    {
        private static readonly CompareInfo CompareInfo = CultureInfo.CurrentCulture.CompareInfo;

        public static int Compare(string? x, string? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            var xIndex = 0;
            var yIndex = 0;
            while (xIndex < x.Length && yIndex < y.Length)
            {
                var xIsDigit = char.IsDigit(x[xIndex]);
                var yIsDigit = char.IsDigit(y[yIndex]);
                if (xIsDigit && yIsDigit)
                {
                    var comparison = CompareNumberRun(x, ref xIndex, y, ref yIndex);
                    if (comparison != 0)
                    {
                        return comparison;
                    }

                    continue;
                }

                var textComparison = CompareTextRun(x, ref xIndex, y, ref yIndex);
                if (textComparison != 0)
                {
                    return textComparison;
                }
            }

            return x.Length.CompareTo(y.Length);
        }

        private static int CompareTextRun(string x, ref int xIndex, string y, ref int yIndex)
        {
            var xStart = xIndex;
            var yStart = yIndex;

            while (xIndex < x.Length && !char.IsDigit(x[xIndex]))
            {
                xIndex++;
            }

            while (yIndex < y.Length && !char.IsDigit(y[yIndex]))
            {
                yIndex++;
            }

            if (xStart == xIndex && xIndex < x.Length)
            {
                xIndex++;
            }

            if (yStart == yIndex && yIndex < y.Length)
            {
                yIndex++;
            }

            return CompareInfo.Compare(
                x[xStart..xIndex],
                y[yStart..yIndex],
                CompareOptions.StringSort);
        }

        private static int CompareNumberRun(string x, ref int xIndex, string y, ref int yIndex)
        {
            var xStart = xIndex;
            var yStart = yIndex;

            while (xIndex < x.Length && char.IsDigit(x[xIndex]))
            {
                xIndex++;
            }

            while (yIndex < y.Length && char.IsDigit(y[yIndex]))
            {
                yIndex++;
            }

            var xSignificant = SkipLeadingZeros(x, xStart, xIndex);
            var ySignificant = SkipLeadingZeros(y, yStart, yIndex);
            var xSignificantLength = xIndex - xSignificant;
            var ySignificantLength = yIndex - ySignificant;

            if (xSignificantLength != ySignificantLength)
            {
                return xSignificantLength.CompareTo(ySignificantLength);
            }

            for (var i = 0; i < xSignificantLength; i++)
            {
                var comparison = x[xSignificant + i].CompareTo(y[ySignificant + i]);
                if (comparison != 0)
                {
                    return comparison;
                }
            }

            return (xIndex - xStart).CompareTo(yIndex - yStart);
        }

        private static int SkipLeadingZeros(string value, int start, int end)
        {
            while (start < end - 1 && value[start] == '0')
            {
                start++;
            }

            return start;
        }
    }
}
