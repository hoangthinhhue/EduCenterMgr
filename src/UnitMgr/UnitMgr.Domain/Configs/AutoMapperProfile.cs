using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace UnitMgr.Domain.Configs;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Add as many of these lines as you need to map your objects
        //CreateMap<TraiChanNuoiDto, TraiChanNuoi>()
        //        .ForMember(x => x.TraiChanNuoi_VatNuoi, opt => opt.Ignore());
        //CreateMap<TraiChanNuoi, TraiChanNuoiDto>()
        //        .ForMember(des => des.TraiChanNuoi_VatNuoi, // Property của DTO
        //                   act => act.MapFrom(src => src.TraiChanNuoi_VatNuoi!.ToList()));

        //CreateMap<AppUserDto, AppUser>().ReverseMap();

    }
}