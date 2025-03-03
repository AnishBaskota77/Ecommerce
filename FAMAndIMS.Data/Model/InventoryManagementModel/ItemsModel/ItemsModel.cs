﻿using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.InventoryManagementModel.ItemsModel
{
    public class ItemsModel : BaseClass
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int UnitId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
