using System;
using JiraWorkLogUploader.Config;
using NPOI.SS.UserModel;

namespace JiraWorkLogUploader.Jira
{
    public class JiraExcelEntry
    {
        public JiraSetting JiraSetting { get; set; }
        public IRow Row { get; set; }

        public DateTime Date { get; set; }
        public double Hours { get; set; }
        public string Issue { get; set; }
        public string Comment { get; set; }

        public void SetUploadResult(int code)
        {
            Row.GetCell(JiraSetting.ColumnNo).SetCellValue(code);
        }
    }
}
