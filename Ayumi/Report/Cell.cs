using System;
using System.Collections.Generic;

namespace Ayumi.Report {
    public class Cell {
        public const String BACKGROUND = "background-color";
        public const String TEXT_COLOR = "color";

        public Object Value;
        public Dictionary<String, String> Style;

        public Cell(Object cellValue) {
            Value = cellValue;
            Style = new Dictionary<String, String>();
        }

        public Cell(Object cellValue, Dictionary<String, String> cellStyle) {
            Value = cellValue;
            Style = cellStyle;
        }
    }
}