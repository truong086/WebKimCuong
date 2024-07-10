using AutoMapper;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Mapper
{
    public class DanhgiaMaper : Profile
    {
        public DanhgiaMaper()
        {
            // Class "DanhgiaDTO" đang chuyền dữ liệu sang cho Class "Danhgia"
            CreateMap<DanhgiaDTO, Danhgia>()
                .ForMember(x => x.message, xd => xd.MapFrom(xdt => xdt.message))
                .ForMember(x => x.sao, xd => xd.MapFrom(xdt => xdt.sao));

            // Class "Danhgia" đang chuyền dữ liệu sang cho Class "DanhgiaDTO"
            CreateMap<Danhgia, DanhgiaDTO>()
                .ForMember(x => x.message, xd => xd.MapFrom(xdt => xdt.message))
                .ForMember(x => x.sao, xd => xd.MapFrom(xdt => xdt.sao));
        }
    }
}
