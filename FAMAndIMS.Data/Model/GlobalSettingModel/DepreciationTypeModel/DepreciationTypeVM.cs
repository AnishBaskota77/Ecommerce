using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.DepreciationModel
{
    public class DepreciationTypeVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "DepreciationTypeName is required.")]
        public string DepreciationTypeName { get; set; }
        [Required(ErrorMessage = "DepreciationTypeCode is required.")]
        public string DepreciationTypeCode { get; set; }
        public bool IsActive { get; set; }
    }
}
