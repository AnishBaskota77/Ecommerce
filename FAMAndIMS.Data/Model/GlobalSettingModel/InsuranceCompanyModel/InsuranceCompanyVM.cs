using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.InsuranceCompanyModel
{
    public class InsuranceCompanyVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string InsuranceCompanyName { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
