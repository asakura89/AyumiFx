using System;
using System.Collections.Generic;

namespace Ayumi.Report {

    public class Row : ICloneable {
        public RowType rowType;
        public List<Cell> Cells;
        private Func<string, int> GetIndexFromOutside;

        internal Row(Func<string, int> parGetIndex)
            : this(parGetIndex, RowType.Data) { }

        internal Row(Func<string, int> parGetIndex, RowType parRowType) {
            GetIndexFromOutside = parGetIndex;
            rowType = parRowType;
            Cells = new List<Cell>();
        }

        public object this[int index] {
            get { return GetCellValue(index); }
            set { SetCellValue(value, index); }
        }

        public object this[string key] {
            get {
                int idx = GetIndexFromOutside(key);
                return GetCellValue(idx);
            }
            set {
                int idx = GetIndex(key);
                SetCellValue(value, idx);
            }
        }

        public Cell GetCellReference(Int32 index) {
            return Cells[index];
        }

        public Cell GetCellReference(String key) {
            Int32 idx = GetIndex(key);
            return Cells[idx];
        }

        private object GetCellValue(int idx) {
            if (idx >= Cells.Count) {
                throw new Exception("Index not exists in row");
            }
            return Cells[idx].Value;
        }

        private void SetCellValue(object value, int idx) {
            for (int i = Cells.Count; i <= idx; i++) {
                Cells.Add(new Cell(null));
            }
            Cells[idx].Value = value;
        }

        private int GetIndex(string key) {
            int idx = GetIndexFromOutside(key);
            if (idx >= Cells.Count) {
                throw new Exception("Index not exists in row");
            }
            return idx;
        }

        public void AddCell(object value) {
            Cells.Add(new Cell(value));
            //AddCell(RowType.DATA, value);
        }

        public void AddCell(object value, Dictionary<String, String> attr) {
            Cells.Add(new Cell(value, attr));
        }

        public object Clone() {
            Row result = new Row(this.GetIndex);
            result.rowType = this.rowType;
            foreach (var cell in Cells) {
                result.AddCell(cell.Value);
            }
            return result;
        }
    }
}