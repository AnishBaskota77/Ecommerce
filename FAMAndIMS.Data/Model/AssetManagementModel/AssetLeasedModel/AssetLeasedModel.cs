using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetLeasedModel
{
    public class AssetLeasedModel : BaseClass
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string PartyName { get; set; }
        public decimal PerDayRate { get; set; }
        public decimal NoOfDays { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
    }
}
