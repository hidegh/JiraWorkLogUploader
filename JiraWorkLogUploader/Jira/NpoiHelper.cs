using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace JiraWorkLogUploader.Jira
{
    public class NpoiHelper
    {
        public static IEnumerable<string> GetSheetNames(string excelFile)
        {
            using (FileStream file = new FileStream(excelFile, FileMode.Open, FileAccess.Read))
            {
                var wb = new XSSFWorkbook(file);

                for (int i = 0; i < wb.NumberOfSheets; i++)
                    yield return wb.GetSheetName(i);
            }
        }

        public static double? GetNumberCellValue(IRow row, int cellNumber)
        {
            var cell = row.GetCell(cellNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            return cell?.NumericCellValue;
        }

        public static DateTime? GetDateCellValue(IRow row, int cellNumber)
        {
            var cell = row.GetCell(cellNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            return cell?.DateCellValue;
        }

        public static bool? GetBoolCellValue(IRow row, int cellNumber)
        {
            var cell = row.GetCell(cellNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            return cell?.BooleanCellValue;
        }

        public static string GetStringCellValue(IRow row, int cellNumber)
        {
            var cell = row.GetCell(cellNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            return cell != null ? cell.StringCellValue : "";
        }
    }
}
