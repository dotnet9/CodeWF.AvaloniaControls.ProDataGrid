using Avalonia.Controls;
using Avalonia.VisualTree;
using CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;
using System.ComponentModel;
using System.Linq;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel? _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => SubscribeViewModel();
        LayoutUpdated += (_, _) => ApplyScrollBarMode();
    }

    private void SubscribeViewModel()
    {
        if (_viewModel is not null)
        {
            _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        _viewModel = DataContext as MainWindowViewModel;

        if (_viewModel is not null)
        {
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        ApplyScrollBarMode();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainWindowViewModel.KeepScrollBarsExpanded))
        {
            ApplyScrollBarMode();
        }
    }

    private void ApplyScrollBarMode()
    {
        var allowAutoHide = _viewModel?.KeepScrollBarsExpanded != true;
        foreach (var scrollViewer in this.GetVisualDescendants().OfType<ScrollViewer>())
        {
            scrollViewer.AllowAutoHide = allowAutoHide;
        }
    }
}
