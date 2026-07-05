using Avalonia;
using Avalonia.Controls;

namespace CodeWF.AvaloniaControls.ProDataGrid;

public sealed class DataGridEnhancement
{
    public static readonly AttachedProperty<bool> UseDefaultEnhancementsProperty =
        AvaloniaProperty.RegisterAttached<DataGridEnhancement, DataGrid, bool>("UseDefaultEnhancements");

    static DataGridEnhancement()
    {
        UseDefaultEnhancementsProperty.Changed.AddClassHandler<DataGrid>(OnUseDefaultEnhancementsChanged);
    }

    private DataGridEnhancement()
    {
    }

    public static bool GetUseDefaultEnhancements(DataGrid dataGrid)
    {
        return dataGrid.GetValue(UseDefaultEnhancementsProperty);
    }

    public static void SetUseDefaultEnhancements(DataGrid dataGrid, bool value)
    {
        dataGrid.SetValue(UseDefaultEnhancementsProperty, value);
    }

    private static void OnUseDefaultEnhancementsChanged(DataGrid dataGrid, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.GetNewValue<bool>())
        {
            dataGrid.ApplyDefaultEnhancements();
        }
    }
}
