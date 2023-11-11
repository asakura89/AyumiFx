using System;
using System.Collections.Generic;

namespace Ayumi.Data
{
    public interface IMultiSelectList
    {
        IList<Int32> SelectedIndexes { get; set; }
        IList<NameValueItem> SelectedItems { get; set; }
        void Add(NameValueItem nvi);
        void AddRange(IList<NameValueItem> nviList);
        void Clear();
        void Remove(Int32 idx);
        void RemoveRange(Func<NameValueItem, Boolean> predicate);
    }
}