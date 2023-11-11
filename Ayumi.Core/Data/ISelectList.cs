using System;
using System.Collections.Generic;

namespace Ayumi.Data
{
    public interface ISelectList
    {
        Int32 SelectedIndex { get; set; }
        NameValueItem SelectedItem { get; set; }
        NameValueItem this[Int32 index] { get; set; }
        void Add(NameValueItem nvi);
        void AddRange(IEnumerable<NameValueItem> nviList);
        void Clear();
        void Remove(Int32 idx);
        void RemoveRange(Func<NameValueItem, Boolean> predicate);
    }
}