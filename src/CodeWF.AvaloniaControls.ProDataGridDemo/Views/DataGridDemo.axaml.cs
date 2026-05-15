using Avalonia.Controls;
using CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.Views;

public partial class DataGridDemo : UserControl
{
    public DataGridDemo()
    {
        InitializeComponent();
        DataGridColumnInitializer.EnsureProcessColumns(ProcessDataGrid);
        DataGridColumnInitializer.ApplyDefaultBehavior(ProcessDataGrid);
    }
}
