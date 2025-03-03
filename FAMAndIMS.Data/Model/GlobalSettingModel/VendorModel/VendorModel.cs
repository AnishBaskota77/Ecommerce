using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.VendorModel
{
    public class VendorModel : BaseClass
    {
        public int Id { get; set; }
        public string VendorName { get; set; }
        public string Address { get; set; }
        public string PanNumber { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
