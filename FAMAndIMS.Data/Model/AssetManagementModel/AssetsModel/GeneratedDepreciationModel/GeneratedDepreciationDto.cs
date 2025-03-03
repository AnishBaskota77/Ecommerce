using FAMAndIMS.Data.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel.GeneratedDepreciationModel
{
    public class GeneratedDepreciationDto : PagedRequest
    {
        public string Event { get; set; }
        public string AssetCode { get; set; }
        public int CategoryId { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public int IsExport { get; set; }
    }
}
