using FAMAndIMS.Data.Model.Paging;
using NPOI.SS.UserModel;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Reflection;
using System.Text;
using ClosedXML.Excel;
using SelectPdf;


namespace FAMAndIMS.Common
{
    public class DataExportHelper
    {
        public static string FileExcelFormat = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static string FileCsvFormat = "text/csv";
        public static (byte[], string fileFormat, string fileName) GenerateCsv<T>(IEnumerable<T> data, string[] columnNames, string topic = null, string fileName = null, bool appendDateTimeToFileName = true)
        {
            var properties = typeof(T).GetProperties();

            var sb = new StringBuilder();
            if (topic is not null || topic == "")
            {
                sb.AppendLine($"\"{topic}\"");
            }

            if (columnNames.Count() <= 0)
            {
                sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));
            }
            else
            {
                sb.AppendLine(string.Join(",", columnNames));
            }


            foreach (var item in data)
            {
                sb.AppendLine(string.Join(",", properties.Select(p =>
                    (p.GetValue(item) ?? "").ToString().Replace(",", ""))));
            }
            fileName = string.IsNullOrWhiteSpace(fileName) ? "ReportCsv" : fileName;
            fileName = appendDateTimeToFileName ? $"{fileName}_{DateTime.Now:yyyyMMddhhmmssfff}.csv" : fileName;
            return (Encoding.UTF8.GetBytes(sb.ToString()), FileCsvFormat, fileName);
        }
        public static Task<(byte[], string fileFormat, string fileName)> ToExcelAsync(DataTable dataTable, string fileName = null, bool appendDateTimeToFileName = true)
        {
            return Task.Run(() =>
            {
                using var workbook = new XLWorkbook();
                workbook.AddWorksheet(dataTable);

                fileName = string.IsNullOrWhiteSpace(fileName) ? "Report" : fileName;
                fileName = appendDateTimeToFileName ? $"{fileName}_{DateTime.Now:yyyyMMddhhmmssfff}.xlsx" : fileName;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);

                return (stream.ToArray(), FileExcelFormat, fileName);
            });
        }

        public static Task<(byte[], string fileFormat, string fileName)> ToExcelAsync(
            List<DataTable> dataTableList, string fileName = null, bool appendDateTimeToFileName = true)
        {
            return Task.Run(() =>
            {
                using var workbook = new XLWorkbook();

                int seedIdx = 1;
                foreach (var dataTable in dataTableList)
                {
                    workbook.AddWorksheet(dataTable, $"Sheet{seedIdx}");
                    seedIdx++;
                }

                fileName = string.IsNullOrWhiteSpace(fileName) ? "Report" : fileName;
                fileName = appendDateTimeToFileName ? $"{fileName}_{DateTime.Now:yyyyMMddhhmmssfff}.xlsx" : fileName;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);

                return (stream.ToArray(), FileExcelFormat, fileName);
            });
        }
        public static async Task<(byte[], string)> TopdfAsync<T>(PagedResponse<T> kyc, string Title)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string header = string.Empty;
            foreach (var property in properties)
            {
                header += $"<th>{property.Name}</th>";

            }

            string htmlhead = "<!DOCTYPE html>\r\n<html>\r\n  <head>\r\n    <title>Title of the document</title>\r\n    <style>\r\n      table,\r\n      th,\r\n      td {\r\n        padding: 10px;\r\n        border: 1px solid black;\r\n        border-collapse: collapse;\r\n      }\r\n    </style>\r\n  </head>\r\n  <body>\r\n " +
               $" <h1>{Title}</h1>" +
               $" <h3 style=\"text-align:right;\">Date :{DateTime.Now:yyyy-MMM-dd hh:mm:ss tt}</h3>" +

               "<table style=\"margin-left: auto; margin-right: auto;\">    " +
               $"<tr>{header}</tr>";

            string htmlbody = string.Empty;
            string tblrow = string.Empty;
            foreach (var data in kyc.Items)
            {

                htmlbody = string.Empty;
                foreach (var porp in properties)
                {
                    htmlbody += $"<td>{porp.GetValue(data)}</td>";
                }
                tblrow += $"<tr>{htmlbody}</tr>";




            }

            string htmlfooter = "</table> </body> </html>";
            string htmldata = htmlhead + tblrow + htmlfooter;

            // instantiate a html to pdf converter object
            HtmlToPdf pdfConverter = new HtmlToPdf();
            pdfConverter.Options.PdfPageSize = PdfPageSize.A4;
            pdfConverter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            pdfConverter.Options.WebPageHeight = 1455;
            //converter.Options.WebPageWidth = 500;
            //converter.Options.WebPageHeight = 100;
            pdfConverter.Options.MarginTop = 5;
            pdfConverter.Options.MarginLeft = 5;
            pdfConverter.Options.MarginRight = 5;
            pdfConverter.Options.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
            pdfConverter.Options.KeepImagesTogether = true;
            pdfConverter.Options.CssMediaType = (HtmlToPdfCssMediaType)Enum.Parse(typeof(HtmlToPdfCssMediaType), "Screen", true);
            // create a new pdf document converting an url
            PdfDocument doc = pdfConverter.ConvertHtmlString(htmldata);

            // save pdf document
            var docBytes = doc.Save();

            // close pdf document
            doc.Close();
            return (docBytes, Application.Pdf); ;
        }
    }
}
