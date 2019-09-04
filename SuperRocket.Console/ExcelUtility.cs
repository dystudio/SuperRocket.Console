using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.SS.UserModel;
using NPOI.Util;
using NPOI.XSSF.UserModel;

namespace SuperRocket.Console
{
    public static class ExcelUtility
    {

        /// <summary>
        /// 将用户数据导入到Excel
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="filepath">导入的文件路径（包含文件名称）</param>
        /// <param name="sheename">要导入的表名</param>
        /// <param name="iscolumwrite">是否写入列名</param>
        /// <returns>导入Excel的行数</returns>
        public static void ExportToExcel(List<User> users, string path, string sheetName, bool isColumnIncluded)
        {
            IWorkbook workbook = null;
            FileStream stream;
            ISheet sheet = null;
            int i = 0;
            int count = 0;
            using (stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (path.IndexOf(".xlsx") > 0) // 2007版本
                {
                    workbook = new XSSFWorkbook();
                }
                else if (path.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook();

                try
                {
                    if (workbook != null)
                    {
                        sheet = workbook.CreateSheet(sheetName);
                    }
                    
                    if (isColumnIncluded) 
                    {
                        IRow row = sheet.CreateRow(0);
                        row.CreateCell(0).SetCellValue("ID");
                        row.CreateCell(1).SetCellValue("Name");
                        count = 1;
                    }
                    
                    for (i = 0; i < users.Count; ++i)
                    {
                        IRow row = sheet.CreateRow(count++);
                        row.CreateCell(0).SetCellValue(users[i].Id.ToString());
                        row.CreateCell(1).SetCellValue(users[i].Name);
                    }
                    workbook.Write(stream);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }
    }
}

