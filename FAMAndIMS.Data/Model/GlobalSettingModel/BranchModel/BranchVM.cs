using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.BranchModel
{
    public class BranchVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Branch Name is required.")]
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        [Required(ErrorMessage = "Address Name is required.")]
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
