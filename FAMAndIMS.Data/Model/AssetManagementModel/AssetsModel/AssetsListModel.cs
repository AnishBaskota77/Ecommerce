using Microsoft.Data.SqlClient.DataClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel
{
    public class AssetsListModel
    {
        public int Id { get; set; }
        public string AssetsName { get; set; }
        public string AssetCode { get; set; }
        public string BarCode { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateBS { get; set; }
        public DateTime AllocateDate { get; set; }
        public string AllocateDateBS { get; set; }
        public string UnitType { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string SerialNumber { get; set; }
        public string Vendor { get; set; }
        public string Employee { get; set; }
        public string DepreciationType { get; set; }
        public string ConditionStatus { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal AccumulatedDepreciationAmount { get; set; }
        public decimal SalvageAmount { get; set; }
        public decimal DepreciationRate { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal AdditionalCostAmount { get; set; }
        public decimal DeletionAmount { get; set; }
        public decimal DepreciationChargeTillLastMonth { get; set; }
        public decimal DepreciationChargeTillToday { get; set; }
        public decimal CurrentMonthChargeAmount { get; set; }
        public decimal ProfitLossAmount { get; set; }
        public decimal MaintenanceCostAmount { get; set; }
        public string UsefulLife { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public string ImageOfAsset { get; set; }
        public DateTime AdditionalCostDate { get; set; }
        public DateTime DeletionDate { get; set; }
        public DateTime DepreciationRunDate { get; set; }
    }

    public class ExportAssetsList
    {
        public string AssetsName { get; set; }
        public string AssetCode { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateBS { get; set; }
        public DateTime AllocateDate { get; set; }
        public string AllocateDateBS { get; set; }
        public string UnitType { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string SerialNumber { get; set; }
        public string Vendor { get; set; }
        public string Employee { get; set; }
        public string DepreciationType { get; set; }
        public string ConditionStatus { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal AccumulatedDepreciationAmount { get; set; }
        public decimal SalvageAmount { get; set; }
        public decimal DepreciationRate { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal AdditionalCostAmount { get; set; }
        public decimal DeletionAmount { get; set; }
        public decimal DepreciationChargeTillLastMonth { get; set; }
        public decimal DepreciationChargeTillToday { get; set; }
        public decimal CurrentMonthChargeAmount { get; set; }
        public decimal ProfitLossAmount { get; set; }
        public decimal MaintenanceCostAmount { get; set; }
        public string UsefulLife { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public string Remarks { get; set; }
        public DateTime AdditionalCostDate { get; set; }
        public DateTime DeletionDate { get; set; }
        public DateTime DepreciationRunDate { get; set; }
    }
}
