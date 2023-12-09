using System;
using System.Collections.Generic;
using System.Linq;

namespace Nvy {
    public class InMemorySelectList : ISelectList {
        IList<NameValueItem> nvies = new List<NameValueItem>();

        public Int32 SelectedIndex { get; set; }
        public NameValueItem SelectedItem { get; set; }

        public NameValueItem this[Int32 index] {
            get { return nvies[index]; }
            set { nvies[index] = value; }
        }

        public IEnumerable<NameValueItem> Items {
            get { return nvies; }
            set { nvies = value.ToList(); }
        }

        public void Add(NameValueItem nvi) => nvies.Add(nvi);

        public void AddRange(IEnumerable<NameValueItem> nviList) {
            foreach (NameValueItem nvy in nviList)
                nvies.Add(nvy);
        }

        public void Clear() => nvies.Clear();

        public void Remove(Int32 idx) => nvies.RemoveAt(idx);

        public void RemoveRange(Func<NameValueItem, Boolean> predicate) {
            IList<NameValueItem> clone = nvies.Where(nvy => true).ToList();
            foreach (NameValueItem nvy in clone)
                if (predicate(nvy))
                    nvies.Remove(nvy);
        }
    }
}
