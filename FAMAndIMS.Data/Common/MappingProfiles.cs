using AutoMapper;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel;
using FAMAndIMS.Data.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Common
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap(typeof(PagedInfo), typeof(PagedResponse<>)).ReverseMap();
            CreateMap<AssetsVM, AssetsModel>()
          .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()) // No matching field in AssetsVM
          .ForMember(dest => dest.ImageOfAssetUrl, opt => opt.Ignore());

            CreateMap<AssetsModel, AssetsVM>()
                .ForMember(dest => dest.ImageOfAsset, opt => opt.Ignore());


        }
    }
}
