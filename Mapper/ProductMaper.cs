using AutoMapper;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Mapper
{
    public class ProductMaper : Profile
    {
        public ProductMaper()
        {
            CreateMap<ProdMst, ProductDTO>()
                .ForMember(a => a.title, b => b.MapFrom(c => c.title))
                .ForMember(a => a.price, b => b.MapFrom(c => c.price))
                .ForMember(a => a.description, b => b.MapFrom(c => c.description))
                .ForMember(a => a.click, b => b.MapFrom(c => c.click));

            CreateMap<ProductDTO, ProdMst>()
                .ForMember(a => a.title, b => b.MapFrom(c => c.title))
                .ForMember(a => a.price, b => b.MapFrom(c => c.price))
                .ForMember(a => a.description, b => b.MapFrom(c => c.description))
                .ForMember(a => a.click, b => b.MapFrom(c => c.click));

            //CreateMap<ProdMst, ProductDTO>()
            //    .ForMember(a => a.title, b => b.MapFrom(c => c.title))
            //    .ForMember(a => a.price, b => b.MapFrom(c => c.price))
            //    .ForMember(a => a.image, b => b.MapFrom(c => Convert.FromBase64String(c.image)))
            //    .ForMember(a => a.description, b => b.MapFrom(c => c.description))
            //    .ForMember(a => a.click, b => b.MapFrom(c => c.click));

            //CreateMap<ProductDTO, ProdMst>()
            //    .ForMember(a => a.title, b => b.MapFrom(c => c.title))
            //    .ForMember(a => a.price, b => b.MapFrom(c => c.price))
            //    .ForMember(a => a.image, b => b.MapFrom(c => Convert.ToBase64String(c.image)))
            //    .ForMember(a => a.description, b => b.MapFrom(c => c.description))
            //    .ForMember(a => a.click, b => b.MapFrom(c => c.click));
        }
    }
}
