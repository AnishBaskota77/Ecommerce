using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetMaintenanceModel
{
    public class AssetMaintenanceVM
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string AssetCode { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal MaintenanceCostAmount { get; set; }
        public bool IsActive { get; set; }
    }
    public class AssetMaintenanceBulkVM
    {
        public int AssetId { get; set; }
        public string AssetCode { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal MaintenanceCostAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
