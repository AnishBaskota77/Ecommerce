using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.InventoryManagementModel.ItemsModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.InventoryManagementServices.ItemsServices
{
    public interface IItemsServices
    {
        Task<PagedResponse<ItemsListModel>> GetItemsList(ItemsDto itemsDto);
        Task<PagedResponse<ExportItemsList>> ExportItemsList(ItemsDto itemsDto);
        Task<SpResponseMessage> SaveItems(ItemsVM itemsVM);
        Task<ItemsVM> GetItemsById(int Id);
        Task<SpResponseMessage> DeleteItems(int id);
    }
}
