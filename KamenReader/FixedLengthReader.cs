﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KamenReader
{
    public sealed class FixedLengthReader : IFileReader
    {
        public FileReaderResult Read(String fullFilepath, IList<FileReaderMap> maps, Boolean firstRowAreTitles = true)
        {
            if (String.IsNullOrEmpty(fullFilepath))
                throw new ArgumentException("fullFilepath");
            if (!File.Exists(fullFilepath))
                throw new InvalidOperationException("File's not found.");
            if (maps == null)
                throw new ArgumentException("maps");
            if (!maps.Any())
                throw new InvalidOperationException("File Reader Map must be supplied.");

            var result = new FileReaderResult();
            String[] allLines = File.ReadAllLines(fullFilepath);
            for (int row = 1; row <= allLines.Length; row++)
            {
                String currentLine = allLines[row - 1];
                Int32 lineLength = currentLine.Length;
                for (int col = 1; col <= maps.Count; col++)
                {
                    FileReaderMap currentMap = maps[col - 1];
                    if (currentMap.StartAt >= lineLength)
                        throw new IndexOutOfRangeException("currentMap.StartAt");

                    String data = currentLine.Substring(currentMap.StartAt, currentMap.Length);
                    String cleaned = String.IsNullOrEmpty(data) ? data : data.Trim();
                    if (firstRowAreTitles && row == 1)
                        result.Titles.Add(cleaned);
                    else
                        result.Data.Add(new GridData { Row = row, Column = currentMap.StartAt, CellValue = cleaned });
                }
            }

            return result;
        }
    }
}