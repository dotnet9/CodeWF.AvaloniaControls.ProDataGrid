using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia;
using Avalonia.VisualTree;
using CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels.Pages;
using System;
using System.Linq;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.Views.Pages;

public partial class DynamicColumnsView : UserControl
{
    public DynamicColumnsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        LayoutUpdated += OnLayoutUpdated;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is DynamicColumnsViewModel viewModel)
        {
            // 这里直接在视图加载完成后触发一次动态列初始化，
            // 避免额外引入行为包，同时让 ProDataGrid 示例保持依赖简单。
            viewModel.RaiseDataGridLoadHandler(MyDataGrid);
        }

        UpdateCornerHeaderWidth();
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        UpdateCornerHeaderWidth();
    }

    private void UpdateCornerHeaderWidth()
    {
        var header = CornerHeaderGrid.GetVisualAncestors().OfType<DataGridColumnHeader>().FirstOrDefault();
        if (MyDataGrid.Columns.Count == 0)
        {
            return;
        }

        var width = header?.Bounds.Width ?? MyDataGrid.Columns[0].ActualWidth;
        var height = header?.Bounds.Height ?? CornerHeaderGrid.Bounds.Height;

        CornerHeaderGrid.Width = Math.Max(0, width + 1);
        CornerHeaderGrid.Height = Math.Max(0, height);

        if (CornerHeaderGrid.Width > 0 && CornerHeaderGrid.Height > 0)
        {
            CornerHeaderDiagonal.StartPoint = new Point(0, 0);
            CornerHeaderDiagonal.EndPoint = new Point(CornerHeaderGrid.Width, CornerHeaderGrid.Height);
        }
    }
}
