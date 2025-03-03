namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetInsuranceModel
{
    public class AssetInsuranceVM
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
    }
    public class AssetInsuranceBulkVM
    {
        public int AssetId { get; set; }
        public int InsuranceCompanyId { get; set; }
        public decimal PremiumAmount { get; set; }
        public string PremiumYear { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string InsuranceType { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
