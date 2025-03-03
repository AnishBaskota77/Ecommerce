using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.MenuManagerModel
{
    public class MenuManagerVM
    {
        public int Id { get; set; }
        public string? Title { get; set; } 
        public int MainParentId { get; set; }
        public int SubParentId { get; set; }
        public string? MenuUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
        public string? IconDataFeather { get; set; } 
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
