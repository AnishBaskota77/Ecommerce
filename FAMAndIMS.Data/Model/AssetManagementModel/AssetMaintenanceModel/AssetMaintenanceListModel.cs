using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetMaintenanceModel
{
    public class AssetMaintenanceListModel
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string AssetCode { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal MaintenanceCostAmount { get; set; }
        public bool IsActive { get; set; }
    }
    public class ExportAssetMaintenanceList
    {
        public string AssetName { get; set; }
        public string AssetCode { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal MaintenanceCostAmount { get; set; }
    }
}
