using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetServicingModel
{
    public class AssetServicingVM
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public DateTime ServicingDate { get; set; }
        public string ServicingDateBS { get; set; }
        public DateTime NextServicingDate { get; set; }
        public string NextServicingDateBS { get; set; }
        public decimal ServicingCharge { get; set; }
        public decimal CurrentDistanceRun { get; set; }
        public string Remarks { get; set; }
        public IFormFile? BillImage { get; set; }
        public string BillImageURL { get; set; }
    }
}
