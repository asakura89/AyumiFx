using System;
using System.Collections.Generic;
using System.Linq;

namespace Ayumi.Report {

    public class ReportSource {
        public Dictionary<String, Column> Columns;
        List<String> SubTotalKeys;
        Dictionary<String, List<Double>> subTotalValues;

        List<Double> grandValues;
        public Boolean IsEnableGrandTotal;

        public Row EmptyRow {
            get { return null; }
        }

        public List<Row> Rows;

        public Row this[Int32 index] {
            get {
                if (index < 0 || index >= Rows.Count)
                    throw new Exception("Index Row not exists in report data");
                return Rows[index];
            }
            set {
                for (Int32 i = Rows.Count; i <= index; i++) {
                    Rows.Add(new Row(GetIndex, RowType.Data));
                }
                Rows[index] = value;
            }
        }

        public Boolean IsAbleToMergeBetweenRow(Int32 columnIndex, Int32 rowIndex, Int32 comparingRowIndex) {
            for (Int32 colIdx = 0; colIdx <= columnIndex; colIdx++) {
                var currentColumn = GetColumnByIndex(colIdx);
                if (currentColumn.IsAbleToMerge == false)
                    continue;
                var cell = Rows[rowIndex].Cells[colIdx];
                var comparingCell = Rows[comparingRowIndex].Cells[colIdx];
                if (!cell.Value.Equals(comparingCell.Value))
                    return false;
            }
            return true;
        }

        public Boolean IsAbleToMergeBetweenColumn(Int32 rowIndex, Int32 columnIndex, Int32 comparingColumnIndex) {
            var currentColumn = GetColumnByIndex(columnIndex);
            var comparingColumn = GetColumnByIndex(comparingColumnIndex);
            if (!currentColumn.IsAbleToMerge)
                return false;
            if (!comparingColumn.IsAbleToMerge)
                return false;

            var cell = Rows[rowIndex].Cells[columnIndex];
            var comparingCell = Rows[rowIndex].Cells[comparingColumnIndex];
            return cell.Value.Equals(comparingCell.Value);
        }

        Column GetColumnByIndex(Int32 idx) {
            var listColumn = Columns.ToList();
            return listColumn[idx].Value;
        }

        public ReportSource() {
            Rows = new List<Row>();
            Columns = new Dictionary<String, Column>();
            SubTotalKeys = new List<String>();
            subTotalValues = new Dictionary<String, List<Double>>();
            grandValues = new List<Double>();
            IsEnableGrandTotal = false;
        }

        public void AddSubTotalKey(params String[] keys) {
            SubTotalKeys.Clear();
            foreach (String key in keys) {
                SubTotalKeys.Add(key);
            }
        }

        public void CalculateSubTotal() {
            List<Row> result = new List<Row>();
            for (Int32 idxRow = 0; idxRow < Rows.Count; idxRow++) {
                Row currentRow = Rows[idxRow];
                if (currentRow.rowType == RowType.Data) {
                    AddSubTotalToRowIfAvailable(idxRow, ref result);
                    AddCurrentRow(result, currentRow);
                    AddToSubTotalValue(idxRow);
                }
                else
                    result.Add(currentRow);
            }
            AddSubTotalToRowAtEnd(ref result);
            if (IsEnableGrandTotal) {
                AddGrandTotalToRow(ref result);
            }
            Rows = result;
        }

        void AddGrandTotalToRow(ref List<Row> result) {
            SetGrandTotalValues();
            Row row = GetDataRow(Rows.Count - 1);
            row = (Row) row.Clone();
            FillWithTextGrandTotal(ref row);
            FillMeasureWithValue(ref row, grandValues);
            result.Add(row);
        }

        void SetGrandTotalValues() {
            grandValues.Clear();
            for (Int32 i = 0; i < Rows.Count; i++) {
                Row currentRow = Rows[i];
                if (currentRow.rowType != RowType.Data)
                    continue;
                CummulativeValueOfMeasureColumn(i, ref grandValues);
            }
        }

        void AddCurrentRow(List<Row> result, Row currentRow) {
            Row resultKey = null;
            if (TryGetRowBasedReportKey(result, currentRow, out resultKey)) {
                AddMeasureColumn(ref resultKey, currentRow);
            }
            else
                result.Add((Row) currentRow.Clone());
        }

        List<Column> GetReportKeyColumn() {
            var result = Columns.ToList().FindAll(p => p.Value.columnType == ColumnType.ReportKey).Select(p => p.Value).ToList();
            return result;
        }

        Boolean TryGetRowBasedReportKey(List<Row> result, Row currentRow, out Row resultKey) {
            resultKey = null;
            for (Int32 idxResult = 0; idxResult < result.Count; idxResult++) {
                if (IsReportKeyEqual(result[idxResult], currentRow)) {
                    resultKey = result[idxResult];
                    return true;
                }
            }
            return false;
        }

        Boolean IsReportKeyEqual(Row row1, Row row2) {
            List<Column> listReportKeyColumn = GetReportKeyColumn();
            foreach (var keyCol in listReportKeyColumn) {
                if (!row1[keyCol.Index].Equals(row2[keyCol.Index]))
                    return false;
            }
            return true;
        }

        void AddMeasureColumn(ref Row resultKey, Row sourceRow) {
            var listMeasureCol = GetMeasureColumn();
            foreach (var measureCol in listMeasureCol) {
                resultKey[measureCol.Index] = Convert.ToDouble(resultKey[measureCol.Index]) + Convert.ToDouble(sourceRow[measureCol.Index]);
            }
        }

        void AddToSubTotalValue(Int32 idxRow) {
            foreach (String key in SubTotalKeys) {
                List<Double> currentSubTotalValue = new List<Double>();
                if (!subTotalValues.ContainsKey(key)) {
                    subTotalValues.Add(key, currentSubTotalValue);
                }
                else {
                    currentSubTotalValue = subTotalValues[key];
                }

                CummulativeValueOfMeasureColumn(idxRow, ref currentSubTotalValue);

                subTotalValues[key] = currentSubTotalValue;
            }
        }

        void CummulativeValueOfMeasureColumn(Int32 idxRow, ref List<Double> result) {
            var listMeasure = GetMeasureColumn();
            FillList<Double>(ref result, listMeasure.Count);

            Int32 idx = 0;
            foreach (var measureCol in listMeasure) {
                Double currentMeasureAmount = Convert.ToDouble(Rows[idxRow][measureCol.Index]);
                result[idx] += currentMeasureAmount;
                idx++;
            }
        }

        static void FillList<T>(ref List<T> list, Int32 numberOfItem) {
            for (Int32 i = list.Count; i < numberOfItem; i++) {
                list.Add(default(T));
            }
        }

        List<Column> GetMeasureColumn() {
            var listColumn = Columns.ToList();
            return listColumn.FindAll(p => p.Value.columnType == ColumnType.Measure).Select(p => p.Value).ToList();
        }

        void AddSubTotalToRowIfAvailable(Int32 idxRow, ref List<Row> result) {
            if (idxRow == 0)
                return;
            Int32 lastIdxRow = GetDataIndex(idxRow - 1);
            Int32 lastIdxRowInResult = GetDataIndex(Rows.Count - 1);
            if (lastIdxRow < 0 || lastIdxRowInResult < 0)
                return;

            foreach (String key in SubTotalKeys) {
                var lastRow = Rows[lastIdxRow];
                var currRow = Rows[idxRow];
                if (!lastRow[key].Equals(currRow[key])) {
                    AddSubTotalFromLastKey(key, idxRow, ref result);
                    return;
                }
            }
        }

        void AddSubTotalToRowAtEnd(ref List<Row> result) {
            Int32 idxRow = Rows.Count;
            foreach (String key in SubTotalKeys) {
                AddSubTotalFromLastKey(key, idxRow, ref result);
                return;
            }
        }

        void AddSubTotalFromLastKey(String key, Int32 idxRow, ref List<Row> result) {
            Int32 idxKey = GetIndexOfSubTotalKey(key);
            for (Int32 idx = SubTotalKeys.Count - 1; idx >= idxKey; idx--) {
                var rowResult = GetSubTotalRowData(SubTotalKeys[idx], idxRow);
                ResetSubTotalValue(SubTotalKeys[idx]);
                result.Add(rowResult);
            }
        }

        void ResetSubTotalValue(String key) {
            List<Double> listValue = subTotalValues[key];
            for (Int32 i = 0; i < listValue.Count; i++) {
                listValue[i] = 0;
            }
        }

        Row GetSubTotalRowData(String key, Int32 idxRow) {
            Row result = (Row) GetDataRow(idxRow - 1).Clone();
            result.rowType = RowType.Total;

            FillKeyValueBeforeKeyParameter(key, idxRow, ref result);
            FillWithTextTotal(key, idxRow, ref result);
            FillWithSubTotalValue(key, idxRow, ref result);
            return result;
        }

        void FillWithSubTotalValue(String key, Int32 idxRow, ref Row result) {
            List<Double> listValue = subTotalValues[key];
            FillMeasureWithValue(ref result, listValue);
        }

        void FillMeasureWithValue(ref Row result, List<Double> listValue) {
            var listMeasureColumn = GetMeasureColumn();
            Int32 idx = 0;
            foreach (var measureCol in listMeasureColumn) {
                result[measureCol.Index] = listValue[idx];
                idx++;
            }
        }

        Row GetDataRow(Int32 rowIdx) {
            Int32 rowDataIdx = GetDataIndex(rowIdx);
            return Rows[rowDataIdx];
        }

        void FillWithTextGrandTotal(ref Row result) {
            var listReportKey = GetReportKeyColumn();
            foreach (var reportKeyColumn in listReportKey) {
                result[reportKeyColumn.Index] = "Total";
            }
        }

        void FillWithTextTotal(String key, Int32 idxRow, ref Row result) {
            Row lastRow = GetDataRow(idxRow - 1);
            result[key] = "Total " + lastRow[key].ToString();
            Int32 idxKey = GetIndexOfSubTotalKey(key);
            var listReportKey = GetReportKeyColumn();
            foreach (var reportKeyColumn in listReportKey) {
                if (reportKeyColumn.Index > idxKey)
                    result[reportKeyColumn.Index] = result[key];
            }
        }

        Int32 GetIndexOfSubTotalKey(String key) {
            Int32 idx = 0;
            foreach (String internalKey in SubTotalKeys) {
                if (internalKey == key)
                    break;
                idx++;
            }
            return idx;
        }

        void FillKeyValueBeforeKeyParameter(String key, Int32 idxRow, ref Row result) {
            var lastRow = GetDataRow(idxRow - 1);
            Int32 idxKey = GetIndexOfSubTotalKey(key);
            for (Int32 idx = 0; idx < idxKey; idx++) {
                result[SubTotalKeys[idx]] = lastRow[SubTotalKeys[idx]];
            }
        }

        public void AddColumn(String title, ColumnType columnType, Boolean isAbleToMerge, params String[] keys) {
            Column data = new Column();
            data.Index = Columns.Count;
            data.Title = title;
            data.IsAbleToMerge = isAbleToMerge;
            data.columnType = columnType;
            String key = GetKeys(keys);
            Columns.Add(key, data);
        }

        public void AddColumnToHeader() {
            Row rowHeader = CreateRow(RowType.Header);
            foreach (var col in Columns.ToList()) {
                rowHeader.AddCell(col.Value.Title);
            }
            Rows.Add(rowHeader);
        }

        public String GetKeys(params String[] keys) {
            String result = String.Join("|", keys);

            return result;
        }

        public void AddHeaderRow(params Object[] paramValue) {
            AddRow(RowType.Header, paramValue);
        }

        public void AddTotalRow(params Object[] paramValue) {
            AddRow(RowType.Total, paramValue);
        }

        public void AddDataRow(Row rowData) {
            AddRow(RowType.Data, rowData);
        }

        public void AddDataRow(params Object[] paramValue) {
            AddRow(RowType.Data, paramValue);
        }

        public void AddRow(RowType rowType, params Object[] paramValue) {
            if (paramValue.Length > Columns.Count) {
                throw new Exception("Columns in this row is greater than defined column");
            }
            Row rowData = CreateRow(rowType);
            foreach (Object value in paramValue) {
                rowData.AddCell(value);
            }
            Rows.Add(rowData);
        }

        public Row LastRowData {
            get {
                Int32 lastIndex = Rows.Count - 1;
                if (lastIndex < 0)
                    throw new Exception("Row still empty");
                while (lastIndex >= 0) {
                    if (Rows[lastIndex].rowType == RowType.Data)
                        return Rows[lastIndex];
                    lastIndex--;
                }
                throw new Exception("Row Data still empty");
            }
        }

        Int32 GetDataIndex(Int32 rowIndex) {
            for (Int32 i = rowIndex; i >= 0; i--) {
                if (Rows[i].rowType == RowType.Data)
                    return i;
            }
            return -1;
        }

        Int32 GetIndex(String key) {
            if (!Columns.ContainsKey(key)) {
                throw new Exception("Invalid Key for Column");
            }
            var result = Columns[key];
            return result.Index;
        }

        public Row CreateRow(RowType rowType) {
            return new Row(GetIndex, rowType);
        }
    }
}