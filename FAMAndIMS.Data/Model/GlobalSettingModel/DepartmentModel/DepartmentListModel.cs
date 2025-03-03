using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.DepartmentModel
{
    public class DepartmentListModel
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set;}
        public bool IsActive { get; set; }
    }
}
