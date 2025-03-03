using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.DepreciationModel
{
    public class DepreciationTypeListModel
    {
        public int Id { get; set; }
        public string DepreciationTypeName { get; set; }
        public string DepreciationTypeCode { get; set; }
        public bool IsActive { get; set; }
    }
}
