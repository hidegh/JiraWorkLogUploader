using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace JiraWorkLogUploader.Excel
{
    public class NpoiHelper
    {
        public static void ThrowIfNOtDesiredType(ICell cell, CellType expectedType, bool allowBlank = true)
        {
            var cellType = GetUnderlyingCellType(cell);

            if (allowBlank && cellType == CellType.Blank)
                return;

            if (cellType != expectedType)
                throw new FormatException($"The cell '{NpoiHelper.RowColumnIndex(cell)}' is of type '{cellType}' whereas type '{expectedType}' is expected!");
        }

        public static CellType GetUnderlyingCellType(ICell cell)
        {
            return cell.CellType == CellType.Formula ? cell.CachedFormulaResultType : cell.CellType;
        }


        public static string RowColumnIndex(ICell cell)
        {
            return $"[{cell.RowIndex},{cell.ColumnIndex}]";
        }

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
            ThrowIfNOtDesiredType(cell, CellType.Numeric);
            return cell.NumericCellValue;
        }

        public static DateTime? GetDateCellValue(IRow row, int cellNumber)
        {
            var cell = row.GetCell(cellNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            ThrowIfNOtDesiredType(cell, CellType.Numeric);
            return cell.DateCellValue;
        }

        public static bool? GetBoolCellValue(IRow row, int cellNumber)
        {
            var cell = row.GetCell(cellNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            ThrowIfNOtDesiredType(cell, CellType.Boolean);
            return cell.BooleanCellValue;
        }

        public static string GetStringCellValue(IRow row, int cellNumber)
        {
            var cell = row.GetCell(cellNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            ThrowIfNOtDesiredType(cell, CellType.String);
            return cell.StringCellValue;
        }

        public static string GetCellAsStringValue(IRow row, int cellNumber)
        {
            var cell = row.GetCell(cellNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell.CellType == CellType.String) return cell.StringCellValue;
            else if (cell.CellType == CellType.Numeric) return cell.NumericCellValue.ToString();
            else if (cell.CellType == CellType.Boolean) return cell.BooleanCellValue.ToString();
            throw new FormatException($"The cell '{NpoiHelper.RowColumnIndex(cell)}' is of type '{cell.CellType}' whereas type 'string|numeric|boolean' is expected!");
        }

    }
}
