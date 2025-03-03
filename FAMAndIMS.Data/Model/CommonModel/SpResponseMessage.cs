using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.CommonModel
{
    public class SpResponseMessage
    {
        public string Msg { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public int ReturnId { get; set; }
        public string MsgType { get; set; } = string.Empty;
    }
}
