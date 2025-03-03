using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.UnitModel
{
    public class UnitVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Unit Name is required.")]
        public string UnitName { get; set; }
        public bool IsActive { get; set; }
    }
}
