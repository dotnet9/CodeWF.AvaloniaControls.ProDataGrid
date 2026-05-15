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
    private const int MaxMetricCount = 9;
    private const int MinGroupCount = 12;
    private const int MaxGroupCount = 20;

    private static readonly string[] MetricNames =
    [
        "温度(℃)",
        "压力(MPa)",
        "电流(A)",
        "电压(V)",
        "转速(rpm)",
        "产量(pcs)",
        "良率(%)",
        "能耗(kWh)",
        "节拍(s)"
    ];

    private bool _isFirstLoadDataGrid = true;
    private bool _isAddingColumns = true;
    private bool _isAddingRows;
    private int _metricCount = 8;
    private Avalonia.Controls.DataGrid? _myDataGrid;
    private int _nextGroupIndex;
    private int _tick;
    private IDisposable? _updateTimerDisposable;

    public DynamicColumnsViewModel()
    {
        for (var i = 0; i < MaxGroupCount; i++)
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
            for (var i = 0; i < group.Items!.Count; i++)
            {
                group.Items[i].Value = CreateMetricValue(i, DynamicGroups.IndexOf(group) + _tick);
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
            Name = $"设备 {_nextGroupIndex:00}",
            Items = []
        };

        for (var i = 0; i < _metricCount; i++)
        {
            group.Items.Add(CreateMetricItem(i, _nextGroupIndex));
        }

        DynamicGroups.Add(group);
        NormalizeGroupNames();
    }

    private static DynamicItem CreateMetricItem(int metricIndex, int groupIndex) => new()
    {
        Key = $"p{metricIndex}",
        Name = MetricNames[metricIndex],
        Value = CreateMetricValue(metricIndex, groupIndex)
    };

    private static string CreateMetricValue(int metricIndex, int sample)
    {
        return metricIndex switch
        {
            0 => $"{22 + sample % 9}.{sample % 10}",
            1 => $"0.{45 + sample % 40}",
            2 => $"{8 + sample % 12}.{sample % 10}",
            3 => $"{218 + sample % 12}",
            4 => $"{900 + sample % 360}",
            5 => $"{120 + sample % 80}",
            6 => $"{96 + sample % 4}.{sample % 10}",
            7 => $"{18 + sample % 10}.{sample % 10}",
            _ => $"{6 + sample % 5}.{sample % 10}"
        };
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Demo page intentionally uses runtime-generated reflection bindings for dynamic columns.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Demo page intentionally uses runtime-generated reflection bindings for dynamic columns.")]
    private static DataGridTemplateColumn CreateMetricColumn(int index) => new()
    {
        CanUserResize = true,
        Header = new TextBlock
        {
            Text = MetricNames[index],
            Margin = new Avalonia.Thickness(12, 0, 0, 0)
        },
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
            NormalizeGroupNames();
        }

        _isAddingRows = DynamicGroups.Count <= MinGroupCount;
    }

    private void NormalizeGroupNames()
    {
        for (var i = 0; i < DynamicGroups.Count; i++)
        {
            DynamicGroups[i].Name = $"设备 {i + 1:00}";
        }
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
