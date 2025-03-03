using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.RoleMenuManagerModel
{
    public class PermissionRole
    {
        public string Name { get; set; }
        public bool Viewper { get; set; }
        public bool Createper { get; set; }
        public bool Updateper { get; set; }
        public bool Deleteper { get; set; }
        public int RoleId { get; set; }
        public int Id { get; set; }
    }
}
