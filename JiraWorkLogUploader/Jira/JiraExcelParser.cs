using System;
using System.IO;
using JiraWorkLogUploader.Config;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace JiraWorkLogUploader.Jira
{
    public class JiraExcelParser
    {
        public XSSFWorkbook Workbook { get; }

        public JiraExcelParser(string excelFileName)
        {
            using (FileStream fileReader = new FileStream(excelFileName, FileMode.Open, FileAccess.Read))
            {
                Workbook = new XSSFWorkbook(fileReader);
            }
        }

        public JiraExcelParser Save(string excelFileName)
        {
            using (var fileWriter = new FileStream(excelFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                Workbook.Write(fileWriter);
            }

            return this;
        }

        private int FindFirstRow(ISheet ws)
        {
            var firstRow = 0;
            var lastRowNum = ws.LastRowNum;
            var currentDate = null as DateTime?;

            for (int r = firstRow; r < lastRowNum; r++)
            {
                // get row
                var row = ws.GetRow(r);
                var firstCellInrow = row.GetCell(0);

                if (firstCellInrow != null && firstCellInrow.StringCellValue == "Day")
                    return r + 1;
            }

            throw new Exception("Could not find cell in column A containing text 'Day'.");
        }

        public void Process(string excelSheet, JiraSetting[] jiras, Action<JiraExcelEntry> entryHandler)
        {
            // get sheet
            var ws = Workbook.GetSheet(excelSheet);

            // iterate through rows
            var firstRow = FindFirstRow(ws);
            var lastRowNum = ws.LastRowNum;
            var currentDate = null as DateTime?;

            for (int r = firstRow; r < lastRowNum; r++)
            {
                // get row
                var row = ws.GetRow(r);

                // skip empty rows
                if (row == null)
                    continue;

                // do we have a new date?
                var dateValue = NpoiHelper.GetDateCellValue(row, 0);
                if (dateValue != null && dateValue != DateTime.MinValue)
                    currentDate = dateValue.Value;

                // if no date, just skip
                if (currentDate == null)
                    continue;

                if (currentDate < new DateTime(2016, 9, 1))
                    throw new NotSupportedException("Dates before 2016 sept are not supported!");

                // get hours
                var hours = NpoiHelper.GetNumberCellValue(row, 1);
                if (hours == null || hours == 0)
                    continue;

                // get task
                var task = NpoiHelper.GetStringCellValue(row, 2);
                if (string.IsNullOrWhiteSpace(task))
                    continue;

                // get comment
                var comment = NpoiHelper.GetStringCellValue(row, 3);

                // get description
                var description = NpoiHelper.GetStringCellValue(row, 4);
                if (string.IsNullOrWhiteSpace(description))
                    description = "no description provided";

                // check entry handled status
                foreach (var jira in jiras)
                {
                    var markerColumnIndex = jira.Column.ToUpper()[0] - 'A';

                    // check and skip if uploaded
                    var uploaded = NpoiHelper.GetNumberCellValue(row, markerColumnIndex);

                    if (uploaded == null || uploaded == 0)
                    {
                        // we got an unhandled entry (which we will probably upload)
                        entryHandler(
                            new JiraExcelEntry()
                            {
                                JiraSetting = jira,
                                Row = row,

                                Date = currentDate.Value,
                                Hours = hours.Value,
                                Issue = task,
                                Comment = description
                            }
                        );
                    }
                    else
                    {
                        // already handled entry - skip upload for other than zero value
                        // < 0 = do not upload
                        // > 0 = http status code (201 = ok) [OK: 200 <= statuscode < 300]
                    }
                }
            }
        }
    }
}