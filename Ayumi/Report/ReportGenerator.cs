using System;
using System.Linq;

namespace Ayumi.Report {

    public class ReportGenerator {
        public ReportSource ReportSource { get; set; }

        public ReportGenerator(ReportSource currentReportSource) {
            ReportSource = currentReportSource;
        }

        public String GenerateAsTable() {
            String tableResult = "";
            for (Int32 rowIdx = 0; rowIdx < ReportSource.Rows.Count; rowIdx++) {
                tableResult += GenerateTableRow(rowIdx);
            }
            String result = ConstructHTMLTable(tableResult);
            return result;
        }

        String GenerateTableRow(Int32 rowIndex) {
            Row row = ReportSource.Rows[rowIndex];
            String rowResult = "";
            for (Int32 cellIdx = 0; cellIdx < row.Cells.Count; cellIdx++) {
                Cell cell = row.Cells[cellIdx];
                if (IsAlreadyVerticalMerge(cellIdx, rowIndex))
                    continue;
                if (IsAlreadyHorizontalMerge(cellIdx, rowIndex))
                    continue;
                String additionalSetting = GetAdditionalSetting(rowIndex, cellIdx);
                if (cell.Style.Count > 0)
                    additionalSetting += GetAdditionalStyle(cell);
                rowResult += ConstructHTMLTableCell(cell.Value, additionalSetting, row.rowType);
            }
            String result = ConstructHTMTableRow(rowResult);
            return result;
        }

        Boolean IsAlreadyHorizontalMerge(Int32 colIndex, Int32 rowIndex) {
            if (colIndex - 1 < 0)
                return false;
            if (ReportSource.IsAbleToMergeBetweenColumn(rowIndex, colIndex, colIndex - 1))
                return true;
            return false;
        }

        String GetAdditionalSetting(Int32 rowIndex, Int32 colIndex) {
            String result = GetAdditionalSettingForVerticalMerging(rowIndex, colIndex);
            result += " " + GetAdditionalSettingForHorizontalMerging(rowIndex, colIndex);
            return result;
        }

        String GetAdditionalStyle(Cell cell) {
            Int32 StyleCount = cell.Style.Count;
            String Style = String.Empty;
            for (Int32 i = 0; i < StyleCount; i++) {
                System.Collections.Generic.KeyValuePair<String, String> cellAttibute = cell.Style.ElementAt(i);
                Style += String.Format("{0}: {1}; ", cellAttibute.Key, cellAttibute.Value);
            }

            String result = String.Format("style=\"{0}\"", Style.Trim(' '));

            return result;
        }

        String GetAdditionalSettingForHorizontalMerging(Int32 rowIndex, Int32 colIndex) {
            Int32 mergeHorizontalCount = GetHorizontalMergingCount(colIndex, rowIndex);

            String additionalSetting = "";
            if (mergeHorizontalCount > 0) {
                additionalSetting = ConstructHTMLHorizontalMergingSetting(mergeHorizontalCount + 1);
            }
            return additionalSetting;
        }

        String ConstructHTMLHorizontalMergingSetting(Int32 mergeHorizontalCount) => String.Format("colspan='{0}'", mergeHorizontalCount);

        Int32 GetHorizontalMergingCount(Int32 colIndex, Int32 rowIndex) {
            Row currentRow = ReportSource.Rows[rowIndex];
            Int32 result = 0;
            for (Int32 cellIdx = colIndex + 1; cellIdx < currentRow.Cells.Count; cellIdx++) {
                if (ReportSource.IsAbleToMergeBetweenColumn(rowIndex, colIndex, cellIdx))
                    result = result + 1;
                else
                    break;
            }
            return result;
        }

        String ConstructHTMLTableCell(Object data, String additionalSetting, RowType rowType) {
            switch (rowType) {
                case RowType.Header:
                    return ConstructHTMLTableCellHeader(data, additionalSetting);

                case RowType.Data:
                    return ConstructHTMLTableCellData(data, additionalSetting);

                case RowType.Total:
                    return ConstructHTMLTableCellData(data, additionalSetting);
            }

            throw new InvalidOperationException("Bug, not complete RowType Implementation");
        }

        String GetAdditionalSettingForVerticalMerging(Int32 rowIndex, Int32 colIndex) {
            Int32 mergeVerticalCount = GetVerticalMergingCount(colIndex, rowIndex);

            String additionalSetting = "";
            if (mergeVerticalCount > 0) {
                additionalSetting = ConstructHTMLVerticalMergingSetting(mergeVerticalCount + 1);
            }
            return additionalSetting;
        }

        Boolean IsAlreadyVerticalMerge(Int32 cellIdx, Int32 rowIndex) {
            if (rowIndex - 1 < 0)
                return false;
            return ReportSource.IsAbleToMergeBetweenRow(cellIdx, rowIndex, rowIndex - 1);
        }

        String ConstructHTMLVerticalMergingSetting(Int32 mergeVerticalCount) => String.Format("rowspan='{0}'", mergeVerticalCount);

        Cell GetCell(Int32 colIndex, Int32 rowIndex) => ReportSource[rowIndex].Cells[colIndex];

        Int32 GetVerticalMergingCount(Int32 colIndex, Int32 rowIndex) {
            Cell currentCell = GetCell(colIndex, rowIndex);
            Int32 result = 0;
            Cell nextCell = null;
            Int32 currentCheckedRow = rowIndex + 1;
            do {
                if (currentCheckedRow >= ReportSource.Rows.Count)
                    return result;

                nextCell = GetCell(colIndex, currentCheckedRow);

                if (!ReportSource.IsAbleToMergeBetweenRow(colIndex, rowIndex, currentCheckedRow))
                    break;
                result = result + 1;
                currentCheckedRow++;
            } while (true);
            return result;
        }

        String ConstructHTMLTable(String tableRows) => String.Format("<table>{0}</table>", tableRows);

        String ConstructHTMTableRow(String cells) => String.Format("<tr>{0}</tr>", cells);

        String ConstructHTMLTableCellHeader(Object data, String additionalSetting) => String.Format("<th class =\"border\" {1}>{0}</th>", data, additionalSetting);

        String ConstructHTMLTableCellData(Object data, String additionalSetting) => String.Format("<td valign=\"top\" class =\"border\" {1}>{0}</td>", data, additionalSetting);
    }
}