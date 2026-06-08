using Avalonia.Controls;
using CodeWF.AvaloniaControls.ProDataGrid;
using CodeWF.AvaloniaControls.ProDataGridDemo.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels.Pages;

public class SortingViewModel : ReactiveObject
{
    private bool _isTriStateSortingEnabled;

    public SortingViewModel()
    {
        var id = 1;
        for (var i = 0; i < 100; i++)
        {
            Students.Add(new Student(id++, "小明", "北京"));
            Students.Add(new Student(id++, "李华", "天津"));
            Students.Add(new Student(id++, "王五", "上海"));
        }
    }

    public ObservableCollection<Student> Students { get; } = new();

    public bool IsTriStateSortingEnabled
    {
        get => _isTriStateSortingEnabled;
        private set
        {
            this.RaiseAndSetIfChanged(ref _isTriStateSortingEnabled, value);
            this.RaisePropertyChanged(nameof(TriStateSortingButtonText));
            this.RaisePropertyChanged(nameof(CanEnableTriStateSorting));
        }
    }

    public bool CanEnableTriStateSorting => !IsTriStateSortingEnabled;

    public string TriStateSortingButtonText => IsTriStateSortingEnabled ? "已启用三态排序" : "启用 AddSorting 扩展";

    public void EnableTriStateSorting(DataGrid dataGrid)
    {
        if (IsTriStateSortingEnabled)
        {
            return;
        }

        dataGrid.AddSorting();
        IsTriStateSortingEnabled = true;
    }
}
