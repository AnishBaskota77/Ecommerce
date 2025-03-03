using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.VendorModel
{
    public class VendorVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vendor Name is required.")]
        public string VendorName { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        public string PanNumber { get; set; }
        public bool IsActive { get; set; }
        public string ContactNumber { get; set; }
    }
}
