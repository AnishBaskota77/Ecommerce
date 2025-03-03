using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.EmployeeModel
{
    public class EmployeeModel : BaseClass
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set;}
    }
}
