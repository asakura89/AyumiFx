using System;
using System.Collections.Generic;

namespace KamenReader {
    public sealed class FileReaderResult {
        public IList<String> Titles { get; set; }
        public IList<GridData> Data { get; set; }

        public FileReaderResult() {
            Titles = new List<String>();
            Data = new List<GridData>();
        }
    }
}