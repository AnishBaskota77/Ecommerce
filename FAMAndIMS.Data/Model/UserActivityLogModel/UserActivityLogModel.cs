
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.UserActivityLogModel
{
    public class UserActivityLogModel
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserAgent { get; set; }
        public string RemoteIpAddress { get; set; }
        public string HttpMethod { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string QueryString { get; set; }
        public bool IsFormData { get; set; }
        public string RequestBody { get; set; }
        public string Headers { get; set; }
        public string RequestUrl { get; set; }
        public string MachineName { get; set; }
        public string Environment { get; set; }
        public string UserAction { get; set; }

    }
}
