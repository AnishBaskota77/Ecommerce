using Microsoft.Data.SqlClient.DataClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.EmployeeModel
{
    public class EmployeeListModel 
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
    }
}
