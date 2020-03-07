using Aspose.Cells;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ILBLI.Core
{
    public class ExcelHelper
    {
        /// <summary>
        /// 将DATATABLE数据进行EXCEL导出
        /// </summary>
        /// <param name="datatable">数据源</param>
        /// <param name="filepath">路径</param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        /// 
        public static byte[] DataTableToExcelByWeb(DataTable datatable)
        {
            try
            {
                if (datatable != null)
                {
                    Workbook workbook = new Workbook();//工作簿
                    Worksheet sheet = workbook.Worksheets[0];//工作簿-工作表
                    Cells cells = sheet.Cells;//工作表单元格对象
                    int nRow = 0;
                    DataColumnCollection columns = datatable.Columns;
                    for (int i = 0; i < columns.Count; i++)
                    {
                        cells[nRow, i].PutValue(Convert.ToString(columns[i].ColumnName));
                    }
                    foreach (DataRow row in datatable.Rows)
                    {
                        nRow++;
                        try
                        {
                            for (int i = 0; i < datatable.Columns.Count; i++)
                            {
                                cells[nRow, i].PutValue(row[i]);
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    return workbook.SaveToStream().ToArray();
                }
            }
            catch (Exception e)
            {

            }
            return new byte[] { };
        }

        /// <summary>
        /// 将DATATABLE数据EXCEL导出_样式版本
        /// </summary>
        /// <param name="datatable"></param>
        /// <returns></returns>
        public static byte[] DataTableToExcelAndStyleByWeb(DataTable datatable)
        {
            Workbook workbook = new Workbook();
            try
            {
                if (datatable != null)
                {
                    Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
                    styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                    styleTitle.Font.Name = "宋体";//文字字体
                    styleTitle.Font.Size = 14;//文字大小
                    ////为单元格添加样式    
                    //Style style = workbook.Styles[workbook.Styles.Add()];
                    ////设置居中
                    //style.HorizontalAlignment = TextAlignmentType.Center;
                    ////设置背景颜色
                    ////style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
                    //style.Pattern = BackgroundType.Solid;//加粗
                    //style.Font.IsBold = true; //黑体//样式设置完毕
                    Cells cells = workbook.Worksheets[0].Cells;
                    int rowIndex = 0;
                    for (int i = 0; i < datatable.Columns.Count; i++)
                    {
                        DataColumn col = datatable.Columns[i];
                        string columnName = col.Caption ?? col.ColumnName;
                        cells[rowIndex, i].PutValue(columnName);
                        cells[rowIndex, i].SetStyle(styleTitle);
                    }
                    rowIndex++;
                    foreach (DataRow row in datatable.Rows)
                    {
                        for (int i = 0; i < datatable.Columns.Count; i++)
                        {
                            cells[rowIndex, i].PutValue(Convert.ToString(row[i]));
                        }
                        rowIndex++;
                    }
                    int columnCount = cells.MaxColumn;  //获取表页的最大列数
                    int rowCount = cells.MaxRow;        //获取表页的最大行数
                    for (int col = 0; col < columnCount; col++)
                    {
                        workbook.Worksheets[0].AutoFitColumn(col, 0, rowCount); //自适应行高
                    }
                    for (int col = 0; col < columnCount; col++)
                    {
                        cells.SetColumnWidthPixel(col, cells.GetColumnWidthPixel(col) + 30);//自适应列宽GetColumnWidthPixel()获取该列的像素值
                    }
                    workbook.Worksheets[0].FreezePanes(1, 0, 1, datatable.Columns.Count);//锁定表格的行列
                    return workbook.SaveToStream().ToArray();
                }
            }
            catch (Exception e)
            {
            }
            return new byte[] { };
        }

        /// <summary>
        /// Excel文件转换为DataTable
        /// </summary>
        /// <param name="filepath">Excel文件的全路径</param>
        /// <param name="datatable">DataTable:返回值</param>
        /// <param name="error">错误信息:返回错误信息，没有错误返回""</param>
        /// <returns>true:函数正确执行 false:函数执行错误</returns>
        public static DataTable ExcelFileToDataTable(string filepath, bool iSTitle = false)
        {
            DataTable datatable = new DataTable();
            try
            {
                if (File.Exists(filepath) == false)
                {
                    datatable = null;
                }
                Workbook workbook = new Workbook(filepath);
                Worksheet worksheet = workbook.Worksheets[0];
                datatable = worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, iSTitle);
                return datatable;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 将EXCEL数据流输出网络流中进行下载 --测试没问题
        /// </summary>
        /// <param name="buffer">数据流</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static HttpResponseMessage ExportExcel(byte[] buffer, string fileName, string type = ".xls")
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(buffer, 0, buffer.Length); // new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName + type
            };
            return response;
        }

        /// <summary>
        /// 月度用资申报汇总表按照模板导出
        /// </summary>
        /// <param name="dt_Master">已签合同数据</param>
        /// <param name="dt_Detail">未签合同数据</param>
        /// <param name="tempPath">模板路径</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="requestTime">申报日期</param>
        /// <returns></returns>
        public static byte[] ExportModule(DataTable dt_Master, DataTable dt_Detail, string tempPath, string requestTime, DataTable timeStr = null)
        {
            try
            {
                if (File.Exists(tempPath))
                {
                    WorkbookDesigner designer = new WorkbookDesigner(new Workbook(tempPath));
                    designer.SetDataSource(dt_Master);
                    designer.SetDataSource(dt_Detail);
                    designer.SetDataSource(timeStr);
                    designer.Process();
                    using (MemoryStream fs = new MemoryStream())
                    {
                        designer.Workbook.Save(fs, SaveFormat.Xlsx);
                        return fs.ToArray();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return new byte[0];
        }
 
        /// <summary>
        /// 将DATATABLE数据EXCEL导出_样式版本
        /// </summary>
        /// <param name="datatable"></param>
        /// <returns></returns>
        public static byte[] DataTableToExcelAndStyleByWebBudgetSum(DataTable datatable)
        {
            Workbook workbook = new Workbook();
            try
            {
                if (datatable != null)
                {

                    Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
                    styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
                    styleTitle.Font.Name = "宋体";//文字字体
                    styleTitle.Font.Size = 14;//文字大小 addMergedRegion

                    ////为单元格添加样式    
                    //Style style = workbook.Styles[workbook.Styles.Add()];
                    ////设置居中
                    //style.HorizontalAlignment = TextAlignmentType.Center;
                    ////设置背景颜色
                    ////style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
                    //style.Pattern = BackgroundType.Solid;//加粗
                    //style.Font.IsBold = true; //黑体//样式设置完毕
                    Cells cells = workbook.Worksheets[0].Cells;

                    int rowIndex = 0;
                    for (int i = 0; i < datatable.Columns.Count; i++)
                    {
                        DataColumn col = datatable.Columns[i];
                        string columnName = col.Caption ?? col.ColumnName;
                        cells[rowIndex, i].PutValue(columnName);
                        cells[rowIndex, i].SetStyle(styleTitle);
                    }
                    rowIndex++;
                    foreach (DataRow row in datatable.Rows)
                    {
                        for (int i = 0; i < datatable.Columns.Count; i++)
                        {
                            //保持原有格式，以方便数字类型数据在excel中操作
                            // cells[rowIndex, i].PutValue(Convert.ToString(row[i]));
                            cells[rowIndex, i].PutValue(row[i]);
                        }
                        rowIndex++;
                    }
                    int columnCount = cells.MaxColumn;  //获取表页的最大列数
                    int rowCount = cells.MaxRow;        //获取表页的最大行数
                    for (int col = 0; col < columnCount; col++)
                    {
                        workbook.Worksheets[0].AutoFitColumn(col, 0, rowCount); //自适应行高
                    }
                    for (int col = 0; col < columnCount; col++)
                    {
                        cells.SetColumnWidthPixel(col, cells.GetColumnWidthPixel(col) + 30);//自适应列宽GetColumnWidthPixel()获取该列的像素值
                        //cells.MergedCells
                    }

                    workbook.Worksheets[0].FreezePanes(1, 6, 1, datatable.Columns.Count);//锁定表格的行列
                    MemoryStream fs = new MemoryStream();
                    workbook.Save(fs, SaveFormat.Xlsx);
                    return fs.ToArray();
                    //return workbook.SaveToStream().ToArray();
                }
            }
            catch (Exception e)
            {
            }
            return new byte[] { };
        }


    }
}
