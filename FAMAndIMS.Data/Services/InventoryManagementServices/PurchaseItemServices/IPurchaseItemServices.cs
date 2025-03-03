using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.InventoryManagementModel.PurchaseItemModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.InventoryManagementServices.PurchaseItemServices
{
    public interface IPurchaseItemServices
    {
        Task<PagedResponse<PurchaseItemListModel>> GetPurchaseItemList(PurchaseItemDto purchaseItemDto);
        Task<PagedResponse<ExportPurchaseItemList>> ExportPurchaseItemList(PurchaseItemDto purchaseItemDto);
        Task<SpResponseMessage> SavePurchaseItem(PurchaseItemVM purchaseItemVM);
        Task<SpResponseMessage> BulkSavePurchaseItems(List<PurchaseItemVM> purchaseItemVM);
        Task<PurchaseItemVM> GetPurchaseItemById(int Id);
        Task<SpResponseMessage> DeletePurchaseItem(int id);
    }
}
