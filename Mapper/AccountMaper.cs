using AutoMapper;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Mapper
{
    public class AccountMaper : Profile
    {
        public AccountMaper()
        {
            CreateMap<UserRegMst, AccountDTO>()
                .ForMember(a => a.email, b => b.MapFrom(c => c.email))
                .ForMember(a => a.username, b => b.MapFrom(c => c.username))
                .ForMember(a => a.phonenumber, b => b.MapFrom(c => c.phonenumber))
                .ForMember(a => a.password, b => b.MapFrom(c => c.password));

            CreateMap<AccountDTO, UserRegMst>()
                .ForMember(a => a.email, b => b.MapFrom(c => c.email))
                .ForMember(a => a.username, b => b.MapFrom(c => c.username))
                .ForMember(a => a.phonenumber, b => b.MapFrom(c => c.phonenumber))
                .ForMember(a => a.password, b => b.MapFrom(c => c.password));

            CreateMap<Roles, RoleDTO>()
                .ForMember(a => a.name, b => b.MapFrom(c => c.name));

            CreateMap<RoleDTO, Roles>()
               .ForMember(a => a.name, b => b.MapFrom(c => c.name));
        }
    }
}
