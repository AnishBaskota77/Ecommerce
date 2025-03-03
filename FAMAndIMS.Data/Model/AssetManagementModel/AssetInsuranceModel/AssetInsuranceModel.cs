using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetInsuranceModel
{
    public class AssetInsuranceModel : BaseClass
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int InsuranceCompanyId { get; set; }
        public decimal PremiumAmount { get; set; }
        public string PremiumYear { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string InsuranceType { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
