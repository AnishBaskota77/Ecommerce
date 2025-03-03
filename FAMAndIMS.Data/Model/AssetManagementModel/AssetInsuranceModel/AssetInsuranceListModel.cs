using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetInsuranceModel
{
    public class AssetInsuranceListModel
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string InsuranceCompanyName {  get; set; }
        public decimal PremiumAmount { get; set; }
        public string PremiumYear { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string InsuranceType { get; set; }
        public bool IsActive { get; set; }
    }

    public class ExportAssetInsuranceList
    {
        public string AssetName { get; set; }
        public string InsuranceCompanyName { get; set; }
        public decimal PremiumAmount { get; set; }
        public string PremiumYear { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string InsuranceType { get; set; }

    }
}
