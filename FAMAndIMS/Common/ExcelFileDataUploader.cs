using FAMAndIMS.Data.Model.GlobalSettingModel.EmployeeModel;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using System.Net;
using System.Reflection;

namespace FAMAndIMS.Common
{
    public static class ExcelFileDataUploader
    {
        public static async Task<List<T>> UploadBulkDataExcel<T>(IFormFile file) where T : new()
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            List<T> records = new List<T>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Read the first worksheet
                    if (worksheet == null)
                    {
                        throw new Exception("No worksheet found.");
                    }

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    PropertyInfo[] properties = typeof(T).GetProperties();

                    for (int row = 2; row <= rowCount; row++) // Assuming the first row is the header
                    {
                        T obj = new T();
                        for (int col = 1; col <= colCount; col++)
                        {
                            string columnName = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                            object cellValue = worksheet.Cells[row, col].Value;

                            PropertyInfo property = properties.FirstOrDefault(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

                            if (property != null && cellValue != null)
                            {
                                try
                                {
                                    object convertedValue = Convert.ChangeType(cellValue, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                                    property.SetValue(obj, convertedValue);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception($"Error converting value '{cellValue}' to property '{property.Name}': {ex.Message}");
                                }
                            }
                        }
                        records.Add(obj);
                    }
                }
            }
            return records;
        }
        public static async Task<HttpResponseMessage> EmployeeBulkExcelUploaderValidation(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("No file uploaded.")
                };
            }

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    if (worksheet == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("No worksheet found.")
                        };
                    }

                    // Expected column headers
                    string[] expectedHeaders = { "EmployeeName", "EmployeeCode", "BranchId", "DepartmentId", "IsActive", "CreatedDate", "UpdatedDate" };

                    // Get the header row from the worksheet (assuming headers are in the first row)
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];

                    // Extract the text values from the header row and trim spaces
                    var actualHeaders = headerRow.Select(cell => cell.Text.Trim()).ToArray();

                    // Check if all expected headers exist in the actual headers (case-insensitive)
                    bool allHeadersExist = expectedHeaders.All(expectedHeader =>
                        actualHeaders.Any(actualHeader => actualHeader.Equals(expectedHeader, System.StringComparison.OrdinalIgnoreCase)));

                    if (!allHeadersExist)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public static async Task<HttpResponseMessage> AssetMaintenanceBulkExcelUploaderValidation(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("No file uploaded.")
                };
            }

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    if (worksheet == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("No worksheet found.")
                        };
                    }

                    // Expected column headers
                    string[] expectedHeaders = { "AssetId", "AssetCode", "MaintenanceDate", "MaintenanceCostAmount", "IsActive", "CreatedDate", "UpdatedDate" };

                    // Get the header row from the worksheet (assuming headers are in the first row)
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];

                    // Extract the text values from the header row and trim spaces
                    var actualHeaders = headerRow.Select(cell => cell.Text.Trim()).ToArray();

                    // Check if all expected headers exist in the actual headers (case-insensitive)
                    bool allHeadersExist = expectedHeaders.All(expectedHeader =>
                        actualHeaders.Any(actualHeader => actualHeader.Equals(expectedHeader, System.StringComparison.OrdinalIgnoreCase)));

                    if (!allHeadersExist)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public static async Task<HttpResponseMessage> AssetInsuranceBulkExcelUploaderValidation(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("No file uploaded.")
                };
            }

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    if (worksheet == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("No worksheet found.")
                        };
                    }

                    // Expected column headers
                    string[] expectedHeaders = { "AssetId", "InsuranceCompanyId", "PremiumAmount", "PremiumYear", "IssueDate", "ExpiryDate", "InsuranceType", "IsActive", "CreatedDate", "UpdatedDate" };

                    // Get the header row from the worksheet (assuming headers are in the first row)
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];

                    // Extract the text values from the header row and trim spaces
                    var actualHeaders = headerRow.Select(cell => cell.Text.Trim()).ToArray();

                    // Check if all expected headers exist in the actual headers (case-insensitive)
                    bool allHeadersExist = expectedHeaders.All(expectedHeader =>
                        actualHeaders.Any(actualHeader => actualHeader.Equals(expectedHeader, System.StringComparison.OrdinalIgnoreCase)));

                    if (!allHeadersExist)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        public static async Task<HttpResponseMessage> AssetAllocationBulkExcelUploaderValidation(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("No file uploaded.")
                };
            }

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    if (worksheet == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("No worksheet found.")
                        };
                    }

                    // Expected column headers
                    string[] expectedHeaders = { "AssetId", "EmployeeId", "BranchId", "DepartmentId", "AllocateDate", "AllocateDateBS", "IsActive", "CreatedDate", "UpdatedDate" };

                    // Get the header row from the worksheet (assuming headers are in the first row)
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];

                    // Extract the text values from the header row and trim spaces
                    var actualHeaders = headerRow.Select(cell => cell.Text.Trim()).ToArray();

                    // Check if all expected headers exist in the actual headers (case-insensitive)
                    bool allHeadersExist = expectedHeaders.All(expectedHeader =>
                        actualHeaders.Any(actualHeader => actualHeader.Equals(expectedHeader, System.StringComparison.OrdinalIgnoreCase)));

                    if (!allHeadersExist)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}