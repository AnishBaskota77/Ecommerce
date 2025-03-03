using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.CategoryModel
{
    public class CategoryVm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required.")]
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Depreciation Rate is required.")]
        public decimal DepreciationRate { get; set; }

    }
}
