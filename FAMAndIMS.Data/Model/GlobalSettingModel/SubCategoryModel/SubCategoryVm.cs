using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.SubCategoryModel
{
    public class SubCategoryVm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "SubCategory Name is required.")]
        public string SubCategoryName { get; set; }
        [Required(ErrorMessage = "Category Name is required.")]
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
