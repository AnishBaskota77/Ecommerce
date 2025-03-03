using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.DepartmentModel
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department Name is required.")]
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public bool IsActive { get; set; }
    }
}
