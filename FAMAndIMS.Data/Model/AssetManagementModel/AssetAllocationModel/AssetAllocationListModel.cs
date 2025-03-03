using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetAllocationModel
{
    public class AssetAllocationListModel
    {
        public int Id { get; set; }
        public string Asset { get; set; }
        public string AssetCode { get; set; }
        public string BarCode { get; set; }
        public string Employee { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
        public DateTime AllocateDate { get; set; }
        public string AllocateDateBS { get; set; }
    }
    public class ExportAssetAllocationList
    {
        public string Asset { get; set; }
        public string AssetCode { get; set; }
        public string Employee { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public DateTime AllocateDate { get; set; }
        public string AllocateDateBS { get; set; }
    }
}
