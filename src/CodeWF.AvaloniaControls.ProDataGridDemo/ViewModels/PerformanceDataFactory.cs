using System;
using System.Collections;
using System.Collections.Generic;
using CodeWF.AvaloniaControls.ProDataGridDemo.Models;

namespace CodeWF.AvaloniaControls.ProDataGridDemo.ViewModels;

internal static class PerformanceDataFactory
{
    public static IReadOnlyList<PerformanceItem> CreateRows(int count, int seed, string scenarioName)
        => new VirtualPerformanceItemList(count, seed, scenarioName);

    private sealed class VirtualPerformanceItemList(int count, int seed, string scenarioName) : IReadOnlyList<PerformanceItem>, IList
    {
        public int Count => count;

        public bool IsFixedSize => true;

        public bool IsReadOnly => true;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public PerformanceItem this[int index] => CreateRow(index);

        object? IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        public IEnumerator<PerformanceItem> GetEnumerator()
        {
            for (var i = 0; i < count; i++)
            {
                yield return CreateRow(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Add(object? value) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public bool Contains(object? value) => IndexOf(value) >= 0;

        public int IndexOf(object? value)
            => value is PerformanceItem item && item.Id > 0 && item.Id <= count ? item.Id - 1 : -1;

        public void Insert(int index, object? value) => throw new NotSupportedException();

        public void Remove(object? value) => throw new NotSupportedException();

        public void RemoveAt(int index) => throw new NotSupportedException();

        public void CopyTo(Array array, int index)
        {
            for (var i = 0; i < count; i++)
            {
                array.SetValue(CreateRow(i), index + i);
            }
        }

        private PerformanceItem CreateRow(int index)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            if (index >= count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return new PerformanceItem(index, seed, scenarioName);
        }
    }
}
