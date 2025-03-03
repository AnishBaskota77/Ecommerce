using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel.GeneratedDepreciationModel
{
    public class GeneratedDepreciationList
    {
        public string AssetName { get; set; }
        public string AssetCode { get; set; }
        public string CategoryName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal AccumulatedDepreciationAmount { get; set; }
        public decimal SalvageAmount { get; set; }
        public decimal DepreciationRate { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal AdditionalCostAmount { get; set; }
        public DateTime AdditionalCostDate { get; set; }
        public decimal DeletionAmount { get; set; }
        public DateTime DeletionDate { get; set; }
        public decimal DepreciationTillLastMonth { get; set; }
        public decimal DepreciationChargeTillToday { get; set; }
        public DateTime DepreciationRunDate { get; set; }
        public decimal CurrentMonthChargeAmount { get; set; }
        public decimal ProfitLossAmount { get; set; }
    }
}
