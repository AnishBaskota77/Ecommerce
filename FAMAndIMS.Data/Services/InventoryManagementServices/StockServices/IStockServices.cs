using FAMAndIMS.Data.Model.InventoryManagementModel.StockModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.InventoryManagementServices.StockServices
{
    public interface IStockServices
    {
        Task<PagedResponse<StockListModel>> GetStockList(StockDto stockDto);
        Task<PagedResponse<ExportStockList>> ExportStockList(StockDto stockDto);
    }
}
