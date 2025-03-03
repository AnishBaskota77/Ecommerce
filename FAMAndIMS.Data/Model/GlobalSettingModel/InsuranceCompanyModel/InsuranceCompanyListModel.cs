using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.GlobalSettingModel.InsuranceCompanyModel
{
    public class InsuranceCompanyListModel
    {
        public int Id { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
