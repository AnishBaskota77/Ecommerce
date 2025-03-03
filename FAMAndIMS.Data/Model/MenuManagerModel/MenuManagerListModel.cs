using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.MenuManagerListModel
{
    public class MenuManagerListModel
    {
        public int Id { get; set; }
        public string MainParentTitle { get; set; } 
        public string SubParentTitle { get; set; }
        public string SubMenuTitle { get; set; }
        public string MenuUrl { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string IconDataFeather { get; set; }

    }
}
