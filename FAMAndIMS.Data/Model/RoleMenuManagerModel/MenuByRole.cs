using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.RoleMenuManagerModel
{
    public class MenuRoleWithChild
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int MainParentId { get; set; }
        public int SubParentId { get; set; }
        public string MenuUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool ViewPer { get; set; }
        public bool CreatePer { get; set; }
        public bool UpdatePer { get; set; }
        public bool DeletePer { get; set; }
        public List<MenuRoleWithChild> SubMenuItems { get; set; } = new List<MenuRoleWithChild>();
    }
    public class RoleMenuPermissionsType
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool ViewPer { get; set; }
        [Required]
        public bool CreatePer { get; set; }
        [Required]
        public bool UpdatePer { get; set; }
        [Required]
        public bool DeletePer { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
