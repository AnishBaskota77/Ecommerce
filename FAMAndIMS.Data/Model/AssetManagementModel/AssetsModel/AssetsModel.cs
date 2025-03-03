using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel
{
    public class AssetsModel : BaseClass
    {
        public int Id { get; set; }
        public string AssetsName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateBS { get; set; }
        public DateTime AllocateDate { get; set; }
        public string AllocateDateBS { get; set; }
        public int UnitTypeId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string SerialNumber { get; set; }
        public int VendorId { get; set; }
        public int EmployeeId { get; set; }
        public int DepreciationTypeId { get; set; }
        public int ConditionStatusId { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
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
        public bool IsDeleted { get; set; }
        public string ImageOfAssetUrl { get; set; }
        public DateTime AdditionalCostDate { get; set; }
        public DateTime DeletionDate { get; set; }
        public DateTime DepreciationRunDate { get; set; }
    }
}
