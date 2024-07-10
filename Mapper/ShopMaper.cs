using AutoMapper;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Mapper
{
    public class ShopMaper : Profile
    {
        public ShopMaper()
        {
            CreateMap<ShopDTO, Inquiry>()
                .ForMember(x => x.Name, d => d.MapFrom(a => a.Name))
                .ForMember(x => x.email, d => d.MapFrom(a => a.email))
                .ForMember(x => x.sodienthoai, d => d.MapFrom(a => a.sodienthoai))
                .ForMember(x => x.diachi, d => d.MapFrom(a => a.diachi))
                .ForMember(x => x.image, d => d.MapFrom(a => a.image));

            CreateMap<Inquiry, ShopDTO>()
                .ForMember(x => x.Name, d => d.MapFrom(a => a.Name))
                .ForMember(x => x.email, d => d.MapFrom(a => a.email))
                .ForMember(x => x.sodienthoai, d => d.MapFrom(a => a.sodienthoai))
                .ForMember(x => x.diachi, d => d.MapFrom(a => a.diachi))
                .ForMember(x => x.image, d => d.MapFrom(a => a.image));
        }
    }
}
