using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ayumi.Data;

namespace Ayumi.Desktop {
    public class DesktopGridView<T> : ICustomList<T> where T : class {
        private readonly DataGridView internalGrid;

        public IEnumerable<Int32> SelectedIndexes {
            get {
                var dataSource = internalGrid.DataSource as List<T>;
                if (dataSource == null)
                    return new List<Int32>();

                return internalGrid.Rows
                    .Cast<DataGridViewRow>()
                    .Where(row => row.Selected)
                    .Select(row => row.Index)
                    .ToList();
            }
            set {
                IEnumerable<Int32> indexList = value;
                foreach (Int32 idx in indexList)
                    internalGrid.Rows[idx].Selected = true;
            }
        }

        public Int32 SelectedIndex {
            get { return SelectedIndexes.FirstOrDefault(); }
            set {
                var indexList = new List<Int32>();
                indexList.Add(value);
                SelectedIndexes = indexList;
            }
        }

        public IEnumerable<T> SelectedItems {
            /*get
            {
                var dataSource = internalGrid.DataSource as List<T>;
                if (dataSource == null)
                    return new List<T>();

                return internalGrid.Rows
                    .Cast<DataGridViewRow>()
                    .Where(row => row.Selected)
                    .Select(row => (T) row.DataBoundItem)
                    .ToList();
            }
            set
            {
                IEnumerable<T> dataList = value;
                foreach (T data in dataList)
                {
                    internalGrid.Rows
                    .Cast<DataGridViewRow>()
                    .Where(row => ((T) row.DataBoundItem).GetHashCode() == data.GetHashCode())
                    .Select(row => { row.Selected = true; row.Index; })
                    .ToList();
                }
                    internalGrid.Rows[idx].Selected = true;
            }*/
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public T SelectedItem {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public T this[int index] {
            get {
                var dataSource = internalGrid.DataSource as List<T>;
                if (dataSource == null)
                    return null;

                return dataSource[index];
            }
            set {
                var dataSource = internalGrid.DataSource as List<T> ?? new List<T>();
                if (!dataSource.Any())
                    dataSource.Add(value);
                else
                    dataSource[index] = value;

                internalGrid.DataSource = dataSource;
            }
        }

        public DesktopGridView(DataGridView grid) {
            if (grid == null)
                throw new ArgumentNullException("grid");

            internalGrid = grid;
        }

        public void Add(T t) {
            var dataSource = internalGrid.DataSource as List<T> ?? new List<T>();
            dataSource.Add(t);
            internalGrid.DataSource = dataSource;
        }

        public void AddRange(IEnumerable<T> tList) {
            var dataSource = internalGrid.DataSource as List<T> ?? new List<T>();
            dataSource.AddRange(tList);
            internalGrid.DataSource = dataSource;
        }

        public void Clear() {
            internalGrid.DataSource = null;
        }
    }
}