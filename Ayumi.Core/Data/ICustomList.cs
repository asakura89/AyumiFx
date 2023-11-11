using System;
using System.Collections.Generic;

namespace Ayumi.Data
{
    public interface ICustomList<T> where T: class
    {
        IEnumerable<Int32> SelectedIndexes { get; set; }
        Int32 SelectedIndex { get; set; }
        IEnumerable<T> SelectedItems { get; set; }
        T SelectedItem { get; set; }
        T this[Int32 index] { get; set; }
        void Add(T t);
        void AddRange(IEnumerable<T> tList);
        void Clear();
    }
}