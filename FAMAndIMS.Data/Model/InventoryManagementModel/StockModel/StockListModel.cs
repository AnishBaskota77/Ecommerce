using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.InventoryManagementModel.StockModel
{
    public class StockListModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal TotPurchaseItem {  get; set; }
        public decimal TotIssuedItem { get; set; }
        public decimal RemainingItem { get; set; }
    }
    public class ExportStockList
    {
        public string ItemName { get; set; }
        public decimal TotPurchaseItem { get; set; }
        public decimal TotIssuedItem { get; set; }
        public decimal RemainingItem { get; set; }
    }
}
