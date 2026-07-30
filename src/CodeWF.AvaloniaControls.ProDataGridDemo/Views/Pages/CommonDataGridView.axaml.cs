using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using CodeWF.AvaloniaControls.ProDataGrid;
using CodeWF.AvaloniaControls.ProDataGridDemo.Models;
using CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels.Pages;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.Views.Pages;

public partial class CommonDataGridView : UserControl
{
    private static readonly IBrush HighlightBrush = SolidColorBrush.Parse("#D9D9D9");
    private CommonDataGridViewModel? _viewModel;

    public CommonDataGridView()
    {
        InitializeComponent();

        ConfigureDataGrid(CommonDataGrid);
        CommonDataGrid.LoadingRow += CommonDataGrid_LoadingRow;
        DataContextChanged += (_, _) => ConfigureSource();
    }

    private void ConfigureSource()
    {
        if (DataContext is not CommonDataGridViewModel viewModel)
        {
            _viewModel = null;
            CommonDataGrid.ItemsSource = Array.Empty<ProcessItem>();
            return;
        }

        if (ReferenceEquals(_viewModel, viewModel))
        {
            return;
        }

        _viewModel = viewModel;
        CommonDataGrid.ItemsSource = viewModel.Records;
        ConfigurePinnedSortComparers(viewModel);
        CommonDataGrid.AddSorting();
        CommonDataGrid.EnableSmartTooltips(4, 5, 6, 7, 8, 9, 10);
        RefreshVisibleRows();
    }

    private static void ConfigureDataGrid(DataGrid dataGrid)
    {
        EnsureColumns(dataGrid);
        dataGrid.ApplyPerformancePreset();
    }

    private void ConfigurePinnedSortComparers(CommonDataGridViewModel viewModel)
    {
        foreach (var column in CommonDataGrid.Columns)
        {
            if (column.CustomSortComparer is IProcessItemPinComparer comparer)
            {
                comparer.ShouldPin = viewModel.ShouldHighlight;
            }
        }
    }

    private static void EnsureColumns(DataGrid dataGrid)
    {
        if (dataGrid.Columns.Count > 0)
        {
            return;
        }

        AddColumn(dataGrid, "编号", nameof(ProcessItem.Id), 90, x => x.Id);
        AddColumn(dataGrid, "任务名称", nameof(ProcessItem.Name), 180, x => x.Name);
        AddColumn(dataGrid, "启用", nameof(ProcessItem.Enabled), 80, x => x.Enabled);
        AddColumn(dataGrid, "源节点", nameof(ProcessItem.SourceNode), 90, x => x.SourceNode);
        AddColumn(dataGrid, "主机", nameof(ProcessItem.Host), 180, x => x.Host);
        AddColumn(dataGrid, "程序路径", nameof(ProcessItem.ProgramPath), 260, x => x.ProgramPath);
        AddColumn(dataGrid, "工作路径", nameof(ProcessItem.WorkPath), 240, x => x.WorkPath);
        AddColumn(dataGrid, "启动参数", nameof(ProcessItem.Params), 220, x => x.Params);
        AddColumn(dataGrid, "前置进程", nameof(ProcessItem.PreProcess), 160, x => x.PreProcess);
        AddColumn(dataGrid, "后置进程", nameof(ProcessItem.PostProcess), 160, x => x.PostProcess);
        AddColumn(dataGrid, "说明", nameof(ProcessItem.Description), 260, x => x.Description);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The sample builds DataGrid columns dynamically in code to demonstrate runtime column setup on Avalonia 12.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The sample builds DataGrid columns dynamically in code to demonstrate runtime column setup on Avalonia 12.")]
    private static void AddColumn<TValue>(DataGrid dataGrid, string header, string path, double width, Func<ProcessItem, TValue?> sortSelector)
    {
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = header,
            Width = new DataGridLength(width),
            SortMemberPath = path,
            Binding = new Binding(path),
            CustomSortComparer = new ProcessItemSortComparer<TValue>(sortSelector)
        });
    }

    private void SetTargetRowBackgroundButton_Click(object? sender, RoutedEventArgs e)
    {
        SetTargetRowBackground();
    }

    private void ClearTargetRowBackgroundButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel is null)
        {
            return;
        }

        _viewModel.ClearTargetRowBackground();
        ReapplySorting();
        RefreshVisibleRows();
    }

    private void TargetValueTextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            SetTargetRowBackground();
            e.Handled = true;
        }
    }

    private void SetTargetRowBackground()
    {
        if (_viewModel is null)
        {
            return;
        }

        _viewModel.TargetValueText = TargetValueTextBox.Text ?? string.Empty;
        if (_viewModel.SetTargetRowBackground())
        {
            ReapplySorting();
            RefreshVisibleRows();
        }
    }

    private void ReapplySorting()
    {
        if (CommonDataGrid.ItemsSource is DataGridCollectionView view)
        {
            view.Refresh();
        }
    }

    private void CommonDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        ApplyRowBackground(e.Row);
    }

    private void RefreshVisibleRows()
    {
        if (_viewModel is null)
        {
            return;
        }

        foreach (var row in CommonDataGrid.GetVisualDescendants().OfType<DataGridRow>())
        {
            ApplyRowBackground(row);
        }
    }

    private void ApplyRowBackground(DataGridRow row)
    {
        if (row.DataContext is ProcessItem item && _viewModel?.ShouldHighlight(item) == true)
        {
            row.Background = HighlightBrush;
        }
        else
        {
            row.ClearValue(TemplatedControl.BackgroundProperty);
        }
    }
}
