using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bharuwa.Common.Utilities
{
    public class ExcelData
    {
        public static DataTable GetData(string filePath)
        {

            return ExcelToDataTable(null, true, filePath);
        }

        private static int LastRowIndex(ISheet aExcelSheet)
        {
            IEnumerator rowIter = aExcelSheet.GetRowEnumerator();
            return rowIter.MoveNext()
            ? aExcelSheet.LastRowNum
            : -1;
        }

        private static int PhysicalRowsCount(ISheet aExcelSheet)
        {
            if (aExcelSheet == null)
            {
                return 0;
            }

            int rowsCount = 0;
            IEnumerator rowEnumerator = aExcelSheet.GetRowEnumerator();
            while (rowEnumerator.MoveNext())
            {
                ++rowsCount;
            }

            return rowsCount;
        }

        public static DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn, string fileName )
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            IWorkbook workbook = null;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //sheetName
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue.Trim();
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //
                    int rowCount = sheet.LastRowNum;

                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //null　　　　　　　
                        if (row.Cells.TrueForAll(u => u.CellType == CellType.Blank)) continue;
                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //null
                                dataRow[j] = row.GetCell(j).ToString().Trim();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                fs.Close();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int DataTableToExcel(DataTable data, string fileName, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            IWorkbook workbook = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                if (fileName.IndexOf(".xlsx") > 0) // 2007
                    workbook = new XSSFWorkbook();
                else if (fileName.IndexOf(".xls") > 0) // 2003
                    workbook = new HSSFWorkbook();
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    if (workbook != null)
                    {
                        sheet = workbook.CreateSheet(sheetName);
                    }
                    else
                    {
                        return -1;
                    }

                    if (isColumnWritten == true) //DataTable
                    {
                        IRow row = sheet.CreateRow(0);
                        for (j = 0; j < data.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName.Trim());
                        }
                        count = 1;
                    }
                    else
                    {
                        count = 0;
                    }

                    for (i = 0; i < data.Rows.Count; ++i)
                    {
                        IRow row = sheet.CreateRow(count);
                        for (j = 0; j < data.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString().Trim());
                        }
                        ++count;
                    }
                    workbook.Write(fs); //excel
                    fs.Flush();
                }
                catch (Exception ex)
                {
                    count = -1;
                }
            }
            return count;
        }

    }
}
