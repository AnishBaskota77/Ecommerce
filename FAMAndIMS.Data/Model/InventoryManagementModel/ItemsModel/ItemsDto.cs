﻿using FAMAndIMS.Data.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.InventoryManagementModel.ItemsModel
{
    public class ItemsDto : PagedRequest
    {
        public int IsExport { get; set; }
    }
}
