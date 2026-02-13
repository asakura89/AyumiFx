using System;
using System.Collections.Generic;

namespace KamenReader {
    public interface IFileReader {
        FileReaderResult Read(String fullFilepath, IList<FileReaderMap> maps, Boolean firstRowAreTitles = true);
    }
}