using AutoMapper;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Mapper
{
    public class CategoryMaper : Profile
    {
        public CategoryMaper()
        {
            CreateMap<CatMst, CategoryDTO>()
                .ForMember(cadto => cadto.images, cas => cas.MapFrom(ca => Convert.FromBase64String(ca.images))) // Convert.FromBase64String() chuyển từ "string" sang mảng "byte[]"
                .ForMember(cadto => cadto.name, cas => cas.MapFrom(ca => ca.name))
                .ForMember(cadto => cadto.creatorId, cas => cas.MapFrom(ca => ca.creatorId));
            CreateMap<CategoryDTO, CatMst>()
                .ForMember(cadto => cadto.name, cas => cas.MapFrom(ca => ca.name))
                .ForMember(cadto => cadto.images, cas => cas.MapFrom(ca => Convert.ToBase64String(ca.images))) // "Convert.ToBase64String()" chuyển từ mảng "byte[]" sang "string"
                .ForMember(cadto => cadto.creatorId, cas => cas.MapFrom(ca => ca.creatorId));
        }
    }
}
