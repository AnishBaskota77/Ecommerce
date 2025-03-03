using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.EmployeeModel
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "EmployeeName is required.")]
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
    public class EmployeeBulkVM
    {
        [Required(ErrorMessage = "EmployeeName is required.")]
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
