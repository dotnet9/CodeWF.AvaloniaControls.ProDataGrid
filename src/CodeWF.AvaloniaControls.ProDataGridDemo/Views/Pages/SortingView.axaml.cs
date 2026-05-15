using Avalonia.Controls;
using Avalonia.Data;
using CodeWF.AvaloniaControls;
using CodeWF.AvaloniaControls.ProDataGridDemo.Models;
using System.Diagnostics.CodeAnalysis;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.Views.Pages;

public partial class SortingView : UserControl
{
    public SortingView()
    {
        InitializeComponent();
        InitializeColumns(StudentDataGrid);
        StudentDataGrid.ApplyPerformancePreset();
    }

    private static void InitializeColumns(DataGrid dataGrid)
    {
        if (dataGrid.Columns.Count > 0)
        {
            return;
        }

        AddColumn(dataGrid, "编号", nameof(Student.Id));
        AddColumn(dataGrid, "姓名", nameof(Student.Name));
        AddColumn(dataGrid, "地区", nameof(Student.Address));
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The sample builds DataGrid columns dynamically in code to demonstrate runtime column setup on Avalonia 12.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The sample builds DataGrid columns dynamically in code to demonstrate runtime column setup on Avalonia 12.")]
    private static void AddColumn(DataGrid dataGrid, string header, string memberPath)
    {
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = header,
            SortMemberPath = memberPath,
            Binding = new Binding(memberPath),
        });
    }
}
