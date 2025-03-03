using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.InventoryManagementModel.PurchaseItemModel
{
    public class PurchaseItemVM
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal VatAmount { get; set; }
        public int VendorId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateBS { get; set; }
        public bool IsActive { get; set; }
    }
}
