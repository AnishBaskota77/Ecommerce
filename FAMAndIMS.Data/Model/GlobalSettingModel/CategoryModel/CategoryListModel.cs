using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.CategoryModel
{
    public class CategoryListModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set;}
        public decimal DepreciationRate { get; set; }
        public bool IsActive { get; set; }
    }
}
