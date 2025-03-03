using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.VendorModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.VendorServices
{
    public interface IVendorServices
    {
        Task<PagedResponse<VendorListModel>> GetVendorList(VendorDto vendorDto);
        Task<SpResponseMessage> SaveVendor(VendorVM vendorVM);
        Task<VendorVM> GetVendorById(int Id);
        Task<SpResponseMessage> DeleteVendor(int id);
    }
}
