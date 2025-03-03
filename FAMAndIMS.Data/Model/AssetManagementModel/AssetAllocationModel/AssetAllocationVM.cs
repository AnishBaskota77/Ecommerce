using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetAllocationModel
{
    public class AssetAllocationVM
    {
        public int Id {  get; set; }
        public int AssetId { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime AllocateDate { get; set; }
        public string AllocateDateBS { get; set; }
        public string AssetCode { get; set; }
        public string BarCodeImageUrl { get; set; }
    }
    public class AssetAllocationBulkVM
    {
        public int AssetId { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public DateOnly AllocateDate { get; set; }
        public string AllocateDateBS { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
