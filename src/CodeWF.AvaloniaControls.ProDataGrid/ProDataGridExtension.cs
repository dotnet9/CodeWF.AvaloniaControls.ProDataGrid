using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Reactive;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CodeWF.AvaloniaControls.ProDataGrid;

public interface IDataGridSortDirectionAwareComparer : IComparer
{
    ListSortDirection SortDirection { get; set; }
}

public static class ProDataGridExtension
{
    private static readonly ConditionalWeakTable<DataGrid, DataGridSortingState> SortingRegistrations = new();
    private static readonly MethodInfo? GetSortPropertyNameMethod =
        typeof(DataGridColumn).GetMethod("GetSortPropertyName", BindingFlags.Instance | BindingFlags.NonPublic);

    /// <summary>
    /// 为 ProDataGrid 启用大数据量场景下更稳妥的默认参数。
    /// </summary>
    public static void ApplyPerformancePreset(this DataGrid dataGrid, bool isReadOnly = true)
    {
        // ProDataGrid 12 的虚拟化与滚动优化建立在逻辑滚动之上，
        // 这里保持启用，确保大数据量页签切换与鼠标滚轮行为一致。
        dataGrid.UseLogicalScrollable = true;
        dataGrid.IsReadOnly = isReadOnly;
        dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
        dataGrid.CanUserReorderColumns = false;
        dataGrid.CanUserResizeColumns = true;
        dataGrid.RowHeight = dataGrid.RowHeight <= 0 ? 36 : dataGrid.RowHeight;
    }

    /// <summary>
    /// 添加三态排序，点击列头时按“未排序、升序、降序”循环切换。
    /// </summary>
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Tri-state sorting relies on DataGridCollectionView reflection behavior provided by ProDataGrid.")]
    public static void AddSorting(this DataGrid dataGrid)
    {
        if (SortingRegistrations.TryGetValue(dataGrid, out _))
        {
            return;
        }

        var state = new DataGridSortingState();
        SortingRegistrations.Add(dataGrid, state);

        dataGrid.Sorting += (s, e) =>
        {
            e.Handled = true;

            if (s is not DataGrid grid)
            {
                return;
            }

            var view = GetOrCreateCollectionView(grid);
            if (view is null)
            {
                state.Clear();
                return;
            }

            var nextDirection = state.GetNextDirection(e.Column);
            ApplyColumnSortDirection(grid, e.Column, nextDirection);
            view.SortDescriptions.Clear();

            if (nextDirection is not null)
            {
                var sortDescription = CreateSortDescription(e.Column, nextDirection.Value, view.Culture);
                if (sortDescription is null)
                {
                    state.Clear();
                }
                else
                {
                    view.SortDescriptions.Add(sortDescription);
                }
            }

            view.Refresh();
        };
    }

    private static void ApplyColumnSortDirection(DataGrid dataGrid, DataGridColumn column, ListSortDirection? direction)
    {
        foreach (var targetColumn in dataGrid.Columns)
        {
            targetColumn.SortDirection = ReferenceEquals(targetColumn, column) ? direction : null;
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Tri-state sorting relies on DataGridCollectionView reflection behavior provided by ProDataGrid.")]
    private static DataGridCollectionView? GetOrCreateCollectionView(DataGrid dataGrid)
    {
        if (dataGrid.ItemsSource is DataGridCollectionView view)
        {
            return view;
        }

        if (dataGrid.ItemsSource is not IEnumerable source)
        {
            return null;
        }

        view = new DataGridCollectionView(source);
        dataGrid.ItemsSource = view;
        return view;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Path-based sorting is part of the ProDataGrid AddSorting helper contract.")]
    private static DataGridSortDescription? CreateSortDescription(
        DataGridColumn column,
        ListSortDirection direction,
        CultureInfo culture)
    {
        if (column.CustomSortComparer is { } comparer)
        {
            if (comparer is IDataGridSortDirectionAwareComparer directionAwareComparer)
            {
                directionAwareComparer.SortDirection = direction;
            }

            return DataGridSortDescription.FromComparer(comparer, direction);
        }

        var memberPath = GetSortPropertyName(column);
        return string.IsNullOrWhiteSpace(memberPath)
            ? null
            : DataGridSortDescription.FromPath(memberPath, direction, culture);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Avalonia DataGrid derives the default sort path from an internal helper.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Avalonia DataGrid derives the default sort path from an internal helper.")]
    private static string? GetSortPropertyName(DataGridColumn column)
    {
        return GetSortPropertyNameMethod?.Invoke(column, null) as string ?? column.SortMemberPath;
    }

    private sealed class DataGridSortingState
    {
        private DataGridColumn? _column;
        private ListSortDirection? _direction;

        public ListSortDirection? GetNextDirection(DataGridColumn column)
        {
            if (!ReferenceEquals(_column, column))
            {
                _column = column;
                _direction = ListSortDirection.Ascending;
                return _direction;
            }

            _direction = _direction switch
            {
                null => ListSortDirection.Ascending,
                ListSortDirection.Ascending => ListSortDirection.Descending,
                ListSortDirection.Descending => null,
                _ => ListSortDirection.Ascending
            };

            if (_direction is null)
            {
                _column = null;
            }

            return _direction;
        }

        public void Clear()
        {
            _column = null;
            _direction = null;
        }
    }

    /// <summary>
    /// 判断是否双击了表格行。
    /// </summary>
    public static bool IsDoubleClickRow(TappedEventArgs? e)
    {
        if (e is null)
        {
            return true;
        }

        if (e.Source is not Control control)
        {
            return false;
        }

        return control.FindAncestorOfType<DataGridRow>() is not null;
    }

    /// <summary>
    /// 为表格启用仅在文字被截断时才显示的智能提示。
    /// </summary>
    public static void EnableSmartTooltips(this DataGrid dataGrid, params int[] targetColumnIndexes)
    {
        dataGrid.LoadingRow += (_, e) => DispatcherTimer.RunOnce(
            () => ProcessDataGridRow(e.Row, targetColumnIndexes),
            TimeSpan.FromMilliseconds(300));
    }

    private static void ProcessDataGridRow(DataGridRow row, int[]? targetColumnIndexes)
    {
        var cells = new List<DataGridCell>();
        FindVisualChildren(row, cells);

        if (targetColumnIndexes?.Any() != true)
        {
            cells.ForEach(ProcessCell);
            return;
        }

        foreach (var columnIndex in targetColumnIndexes)
        {
            if (columnIndex < 0 || columnIndex >= cells.Count)
            {
                continue;
            }

            ProcessCell(cells[columnIndex]);
        }
    }

    private static void ProcessCell(DataGridCell cell)
    {
        var textBlocks = new List<TextBlock>();
        FindVisualChildren(cell, textBlocks);
        textBlocks.ForEach(SetupSmartTooltip);
    }

    private static void FindVisualChildren<T>(Visual visual, List<T> array)
        where T : Visual
    {
        foreach (var child in visual.GetVisualChildren())
        {
            if (child is T match)
            {
                array.Add(match);
            }

            FindVisualChildren(child, array);
        }
    }

    private static void SetupSmartTooltip(TextBlock textBlock)
    {
        if (textBlock.Tag is true)
        {
            return;
        }

        // 使用 Tag 做一次性标记，避免重复订阅同一个 TextBlock。
        textBlock.Tag = true;
        textBlock.TextTrimming = TextTrimming.CharacterEllipsis;

        UpdateToolTip(textBlock);
        textBlock.GetObservable(TextBlock.TextProperty)
            .Subscribe(new AnonymousObserver<string?>(_ => UpdateToolTip(textBlock)));
        textBlock.GetObservable(Visual.BoundsProperty)
            .Subscribe(new AnonymousObserver<Rect>(_ => UpdateToolTip(textBlock)));
    }

    private static void UpdateToolTip(TextBlock textBlock)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(textBlock.Text) || textBlock.Bounds.Width <= 0)
            {
                ToolTip.SetTip(textBlock, null);
                return;
            }

            var formattedText = new FormattedText(
                textBlock.Text!,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight),
                textBlock.FontSize,
                Brushes.Black);

            ToolTip.SetTip(textBlock, formattedText.Width > textBlock.Bounds.Width ? textBlock.Text : null);
        }
        catch
        {
            // 样例工具提示只做增强，不让异常影响主交互。
        }
    }
}
