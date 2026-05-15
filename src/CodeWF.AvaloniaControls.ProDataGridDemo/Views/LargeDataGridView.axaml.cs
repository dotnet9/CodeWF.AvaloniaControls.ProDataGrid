using Avalonia.Controls;
using CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.Views;

public partial class LargeDataGridView : UserControl
{
    private bool _initialized;

    public LargeDataGridView()
    {
        InitializeComponent();
        Loaded += (_, _) => EnsureInitialized();
    }

    private void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        if (DataContext is LargeDataGridViewModel viewModel)
        {
            ProcessDataGrid.ItemsSource = viewModel.Items;
        }

        DataGridColumnInitializer.EnsureProcessColumns(ProcessDataGrid);
        DataGridColumnInitializer.ApplyDefaultBehavior(ProcessDataGrid);

        _initialized = true;
    }
}
