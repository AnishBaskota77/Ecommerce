using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetAllocationModel
{
    public class AssetAllocationModel : BaseClass
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string BarCodeImageUrl { get; set; }

        public int AssetId { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime AllocateDate { get; set; }
        public string AllocateDateBS { get; set; }
        public bool IsDeleted { get; set; }
    }
}
