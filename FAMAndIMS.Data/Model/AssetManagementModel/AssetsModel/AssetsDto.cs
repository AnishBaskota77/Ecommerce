﻿using FAMAndIMS.Data.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel
{
    public class AssetsDto : PagedRequest
    {
        public int IsExport { get; set; }
    }
}
