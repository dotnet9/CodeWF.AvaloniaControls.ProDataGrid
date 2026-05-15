using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Threading;
using CodeWF.AvaloniaControls.ProDataGridDemo.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels.Pages;

public class DynamicColumnsViewModel : ReactiveObject, IDisposable
{
    private const int MinMetricCount = 5;
    private const int MaxMetricCount = 12;
    private const int MinGroupCount = 16;
    private const int MaxGroupCount = 30;

    private bool _isFirstLoadDataGrid = true;
    private bool _isAddingColumns = true;
    private bool _isAddingRows = true;
    private int _metricCount = 8;
    private Avalonia.Controls.DataGrid? _myDataGrid;
    private int _nextGroupIndex;
    private int _tick;
    private IDisposable? _updateTimerDisposable;

    public DynamicColumnsViewModel()
    {
        for (var i = 0; i < 18; i++)
        {
            AddGroup();
        }

        _updateTimerDisposable = Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => Dispatcher.UIThread.Post(UpdateDynamicItemValues));
    }

    private void UpdateDynamicItemValues()
    {
        _tick++;

        foreach (var group in DynamicGroups)
        {
            foreach (var item in group.Items!)
            {
                item.Value = $"数值 {Random.Shared.Next(1000, 9999)}";
            }

            group.RaisePropertyChanged(nameof(DynamicGroup.Items));
        }

        if (_tick % 2 == 0)
        {
            UpdateDynamicRows();
        }

        if (_tick % 3 == 0)
        {
            UpdateDynamicColumns();
        }
    }

    public void Dispose()
    {
        _updateTimerDisposable?.Dispose();
    }

    public ObservableCollection<DynamicGroup> DynamicGroups { get; } = [];

    public void RaiseDataGridLoadHandler(Avalonia.Controls.DataGrid dataGrid)
    {
        _myDataGrid = dataGrid;
        InitDynamicDataGrid();
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Demo page intentionally uses runtime-generated reflection bindings for dynamic columns.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Demo page intentionally uses runtime-generated reflection bindings for dynamic columns.")]
    private void InitDynamicDataGrid()
    {
        if (_myDataGrid == null || !_isFirstLoadDataGrid || !DynamicGroups.Any() ||
            DynamicGroups.First().Items?.Any() != true)
        {
            return;
        }

        _isFirstLoadDataGrid = false;

        var dynamicColumns = DynamicGroups.First().Items!.Select((_, index) => CreateMetricColumn(index));

        foreach (var column in dynamicColumns)
        {
            _myDataGrid.Columns.Add(column);
        }
    }

    private void AddGroup()
    {
        _nextGroupIndex++;
        var group = new DynamicGroup
        {
            Name = $"分组 {_nextGroupIndex}",
            Items = []
        };

        for (var i = 0; i < _metricCount; i++)
        {
            group.Items.Add(CreateMetricItem(i, _nextGroupIndex));
        }

        DynamicGroups.Add(group);
    }

    private static DynamicItem CreateMetricItem(int metricIndex, int groupIndex) => new()
    {
        Key = $"p{metricIndex}",
        Name = $"指标 {metricIndex + 1}",
        Value = $"数值 {groupIndex}-{metricIndex + 1}"
    };

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Demo page intentionally uses runtime-generated reflection bindings for dynamic columns.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Demo page intentionally uses runtime-generated reflection bindings for dynamic columns.")]
    private static DataGridTemplateColumn CreateMetricColumn(int index) => new()
    {
        CanUserResize = true,
        Header = $"指标 {index + 1}",
        IsReadOnly = true,
        MinWidth = 80,
        Width = new DataGridLength(120),
        CellTemplate = new FuncDataTemplate<DynamicGroup>((_, _) => new TextBlock
        {
            Classes = { "Content" },
            [!TextBlock.TextProperty] = new Binding($"Items[{index}].Value")
        })
    };

    private void UpdateDynamicRows()
    {
        if (_isAddingRows)
        {
            AddGroup();
            _isAddingRows = DynamicGroups.Count < MaxGroupCount;
            return;
        }

        if (DynamicGroups.Count > MinGroupCount)
        {
            DynamicGroups.RemoveAt(DynamicGroups.Count - 1);
        }

        _isAddingRows = DynamicGroups.Count <= MinGroupCount;
    }

    private void UpdateDynamicColumns()
    {
        if (_myDataGrid == null)
        {
            return;
        }

        if (_isAddingColumns)
        {
            foreach (var group in DynamicGroups)
            {
                group.Items!.Add(CreateMetricItem(_metricCount, DynamicGroups.IndexOf(group) + 1));
                group.RaisePropertyChanged(nameof(DynamicGroup.Items));
            }

            _myDataGrid.Columns.Add(CreateMetricColumn(_metricCount));
            _metricCount++;
            _isAddingColumns = _metricCount < MaxMetricCount;
            return;
        }

        if (_metricCount > MinMetricCount && _myDataGrid.Columns.Count > 1)
        {
            _metricCount--;

            foreach (var group in DynamicGroups)
            {
                group.Items!.RemoveAt(group.Items.Count - 1);
                group.RaisePropertyChanged(nameof(DynamicGroup.Items));
            }

            _myDataGrid.Columns.RemoveAt(_myDataGrid.Columns.Count - 1);
        }

        _isAddingColumns = _metricCount <= MinMetricCount;
    }
}
