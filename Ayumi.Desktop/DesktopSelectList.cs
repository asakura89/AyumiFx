using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ayumi.Data;

namespace Ayumi.Desktop {
    public class DesktopSelectList : ISelectList {
        private readonly ListControl internalList;

        public Int32 SelectedIndex {
            get { return internalList.SelectedIndex; }
            set { internalList.SelectedIndex = value; }
        }

        public NameValueItem SelectedItem {
            get { return this[internalList.SelectedIndex]; }
            set { this[internalList.SelectedIndex] = value; }
        }

        public NameValueItem this[Int32 index] {
            get {
                var dataSource = internalList.DataSource as List<NameValueItem>;
                if (dataSource == null)
                    return null;

                return dataSource[index];
            }
            set {
                var dataSource = internalList.DataSource as List<NameValueItem> ?? new List<NameValueItem>();
                if (!dataSource.Any())
                    dataSource.Add(value);
                else
                    dataSource[index] = value;

                internalList.DataSource = dataSource;
            }
        }

        public DesktopSelectList(ListControl listControl) {
            if (listControl == null)
                throw new ArgumentNullException("listControl");

            internalList = listControl;
        }

        public void Add(NameValueItem nvi) {
            var dataSource = internalList.DataSource as List<NameValueItem> ?? new List<NameValueItem>();
            dataSource.Add(nvi);
            internalList.PopulateWithRawDataList(dataSource, NameValueItem.NameProperty, NameValueItem.ValueProperty);
        }

        public void AddRange(IEnumerable<NameValueItem> nviList) {
            var dataSource = internalList.DataSource as List<NameValueItem> ?? new List<NameValueItem>();
            dataSource.AddRange(nviList);
            internalList.PopulateWithRawDataList(dataSource, NameValueItem.NameProperty, NameValueItem.ValueProperty);
        }

        public void Clear() {
            internalList.DataSource = null;
        }

        public void Remove(Int32 idx) {
            var dataSource = internalList.DataSource as List<NameValueItem> ?? new List<NameValueItem>();
            dataSource.RemoveAt(idx);
            internalList.PopulateWithRawDataList(dataSource, NameValueItem.NameProperty, NameValueItem.ValueProperty);
        }

        public void RemoveRange(Func<NameValueItem, Boolean> predicate) {
            var dataSource = internalList.DataSource as List<NameValueItem> ?? new List<NameValueItem>();
            dataSource.RemoveAll(nvi => predicate(nvi));
            internalList.PopulateWithRawDataList(dataSource, NameValueItem.NameProperty, NameValueItem.ValueProperty);
        }
    }
}