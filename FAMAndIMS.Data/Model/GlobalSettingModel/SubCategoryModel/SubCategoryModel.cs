using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.SubCategoryModel
{
    public class SubCategoryModel : BaseClass
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
