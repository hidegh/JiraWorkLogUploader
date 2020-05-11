using System;
using JiraWorkLogUploader.Config;
using NPOI.SS.UserModel;

namespace JiraWorkLogUploader.Excel
{
    public class WorkLogEntry
    {
        public ExportSettings ExportSettings { get; set; }
        public IRow Row { get; set; }

        public DateTime Date { get; set; }
        public double Hours { get; set; }
        public string Issue { get; set; }
        public string Comment { get; set; }

        public void SetUploadResult(int code)
        {
            Row.GetCell(ExportSettings.ColumnNo).SetCellValue(code);
        }
    }
}
