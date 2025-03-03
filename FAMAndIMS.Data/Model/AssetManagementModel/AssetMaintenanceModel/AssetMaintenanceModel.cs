using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetMaintenanceModel
{
    public class AssetMaintenanceModel : BaseClass
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string AssetCode { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal MaintenanceCostAmount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
