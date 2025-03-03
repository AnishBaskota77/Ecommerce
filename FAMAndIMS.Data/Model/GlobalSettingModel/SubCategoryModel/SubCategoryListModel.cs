using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.SubCategoryModel
{
    public class SubCategoryListModel
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}
