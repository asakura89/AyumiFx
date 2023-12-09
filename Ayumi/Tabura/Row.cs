using System;
using System.Collections.Generic;

namespace Tabura {

    public class Row : ICloneable {
        public RowType rowType;
        public List<Cell> Cells;
        private Func<String, Int32> GetIndexFromOutside;

        internal Row(Func<String, Int32> parGetIndex)
            : this(parGetIndex, RowType.Data) { }

        internal Row(Func<String, Int32> parGetIndex, RowType parRowType) {
            GetIndexFromOutside = parGetIndex;
            rowType = parRowType;
            Cells = new List<Cell>();
        }

        public Object this[Int32 index] {
            get { return GetCellValue(index); }
            set { SetCellValue(value, index); }
        }

        public Object this[String key] {
            get {
                Int32 idx = GetIndexFromOutside(key);
                return GetCellValue(idx);
            }
            set {
                Int32 idx = GetIndex(key);
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

        private Object GetCellValue(Int32 idx) {
            if (idx >= Cells.Count) {
                throw new Exception("Index not exists in row");
            }

            return Cells[idx].Value;
        }

        private void SetCellValue(Object value, Int32 idx) {
            for (Int32 i = Cells.Count; i <= idx; i++) {
                Cells.Add(new Cell(null));
            }

            Cells[idx].Value = value;
        }

        private Int32 GetIndex(String key) {
            Int32 idx = GetIndexFromOutside(key);
            if (idx >= Cells.Count) {
                throw new Exception("Index not exists in row");
            }

            return idx;
        }

        public void AddCell(Object value) {
            Cells.Add(new Cell(value));
            //AddCell(RowType.DATA, value);
        }

        public void AddCell(Object value, Dictionary<String, String> attr) {
            Cells.Add(new Cell(value, attr));
        }

        public Object Clone() {
            var result = new Row(GetIndex) { rowType = rowType };
            foreach (Cell cell in Cells) {
                result.AddCell(cell.Value);
            }

            return result;
        }
    }
}