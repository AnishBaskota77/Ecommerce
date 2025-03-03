using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using FAMAndIMS.Data.Model.GlobalSettingModel.CategoryModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.InsuranceCompanyModel;
using FAMAndIMS.Data.Services.AdminLoginServices;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetAllocationServices;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetInsuranceServices;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetLeasedServices;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetMaintenanceServices;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetServicingServices;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetsServices;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.EmployeeManagementServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.BranchServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.CategoryServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.DepartmentServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.DepreciationServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.EmployeeServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.InsuranceCompanyServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.SubCategoryServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.UnitServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.VendorServices;
using FAMAndIMS.Data.Services.InventoryManagementServices.ItemsServices;
using FAMAndIMS.Data.Services.InventoryManagementServices.PurchaseItemServices;
using FAMAndIMS.Data.Services.InventoryManagementServices.StockServices;
using FAMAndIMS.Data.Services.MenuServices;
using FAMAndIMS.Data.Services.RoleMenuManagerServices;
using FAMAndIMS.Data.Services.UserActivityLogServices;
using Microsoft.AspNetCore.Hosting;

namespace FAMAndIMS.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMenuManagerService, MenuMangerService>();
            services.AddScoped<ICommonddlService, CommonddlService>();
            services.AddScoped<IRoleMenuMangerService, RoleMenuManagerService>();
            services.AddScoped<IAdminUserManagementServices, AdminUserManagementServices>();
            services.AddScoped<ICommonServices, CommonServices>();
            services.AddScoped<IAdminLoginService, AdminLoginService>();
            services.AddScoped<IUserActivityLogs, UserActivityLogs>();
            services.AddScoped<IDepartmentServices, DepartmentServices>();
            services.AddScoped<IBranchServices, BranchServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ISubCategoryServices, SubCategoryServices>();
            services.AddScoped<IUnitServices, UnitServices>();
            services.AddScoped<IVendorServices, VendorServices>();
            services.AddScoped<IInsuranceCompanyServices, InsuranceCompanyServices>();
            services.AddScoped<IEmployeeServices, EmployeeServices>();
            services.AddScoped<IDepreciationTypeServices, DepreciationTypeServices>();
            services.AddScoped<IAssetMaintenanceServices, AssetMaintenanceServices>();
            services.AddScoped<IAssetInsuranceServices, AssetInsuranceServices>();
            services.AddScoped<IAssetAllocationServices, AssetAllocationServices>();
            services.AddScoped<IAssetsServices, AssetsServices>();
            services.AddScoped<IItemsServices, ItemsServices>();
            services.AddScoped<IPurchaseItemServices, PurchaseItemServices>();
            services.AddScoped<IStockServices, StockServices>();
            services.AddScoped<IAssetServicingServices, AssetServicingServices>();
            services.AddScoped<IAssetLeasedServices, AssetLeasedServices>();
            //for notification
            services.AddScoped<INotyfService, NotyfService>();


            return services;
        }
    }
}
