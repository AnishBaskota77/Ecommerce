using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.InventoryManagementModel.PurchaseItemModel
{
    public class PurchaseItemListModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal VatAmount {  get; set; }
        public string Vendor {  get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateBS { get; set; }
        public bool IsActive { get; set; }
    }
    public class ExportPurchaseItemList
    {
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal VatAmount { get; set; }
        public string Vendor { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateBS { get; set; }
    }
}
