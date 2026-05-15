using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.VisualTree;
using CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;
using System.ComponentModel;
using System.Linq;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel? _viewModel;
    private bool _isApplyingScrollBarMode;

    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => SubscribeViewModel();
        Loaded += (_, _) => ApplyScrollBarMode();
        LayoutUpdated += (_, _) =>
        {
            if (_viewModel?.KeepScrollBarsExpanded == true)
            {
                ApplyScrollBarMode();
            }
        };
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
        if (_isApplyingScrollBarMode)
        {
            return;
        }

        _isApplyingScrollBarMode = true;
        var expanded = _viewModel?.KeepScrollBarsExpanded == true;

        try
        {
            if (expanded && !Classes.Contains("ScrollBarsExpanded"))
            {
                Classes.Add("ScrollBarsExpanded");
            }
            else if (!expanded)
            {
                Classes.Remove("ScrollBarsExpanded");
            }

            foreach (var scrollViewer in this.GetVisualDescendants().OfType<ScrollViewer>())
            {
                ApplyScrollViewerMode(scrollViewer, expanded);
            }

            foreach (var scrollBar in this.GetVisualDescendants().OfType<ScrollBar>())
            {
                ApplyScrollBarMode(scrollBar, expanded);
            }
        }
        finally
        {
            _isApplyingScrollBarMode = false;
        }
    }

    private static void ApplyScrollViewerMode(ScrollViewer scrollViewer, bool expanded)
    {
        if (expanded)
        {
            scrollViewer.AllowAutoHide = false;
            return;
        }

        scrollViewer.ClearValue(ScrollViewer.AllowAutoHideProperty);
    }

    private static void ApplyScrollBarMode(ScrollBar scrollBar, bool expanded)
    {
        if (!expanded)
        {
            scrollBar.ClearValue(ScrollBar.AllowAutoHideProperty);
            scrollBar.ClearValue(Layoutable.WidthProperty);
            scrollBar.ClearValue(Layoutable.MinWidthProperty);
            scrollBar.ClearValue(Layoutable.HeightProperty);
            scrollBar.ClearValue(Layoutable.MinHeightProperty);
            scrollBar.ClearValue(Visual.OpacityProperty);
            return;
        }

        const double expandedSize = 16;

        scrollBar.AllowAutoHide = false;
        scrollBar.Opacity = 1;

        if (scrollBar.Orientation == Orientation.Vertical)
        {
            scrollBar.Width = expandedSize;
            scrollBar.MinWidth = expandedSize;
            return;
        }

        scrollBar.Height = expandedSize;
        scrollBar.MinHeight = expandedSize;
    }
}
