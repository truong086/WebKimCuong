using AutoMapper;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Mapper
{
    public class CommentMaper : Profile
    {
        public CommentMaper()
        {
            CreateMap<Comment, CommentDTO>()
                .ForMember(cto => cto.images, c => c.MapFrom(com => com.images))
                .ForMember(cto => cto.message, c => c.MapFrom(com => com.message));

            CreateMap<CommentDTO, Comment>()
                .ForMember(cto => cto.images, c => c.MapFrom(com => com.images))
                .ForMember(cto => cto.message, c => c.MapFrom(com => com.message));

            CreateMap<CommentDescription, CommentDescriptionDTO>()
                .ForMember(cto => cto.images, c => c.MapFrom(com => Convert.ToBase64String(com.images)))
                .ForMember(cto => cto.message, c => c.MapFrom(com => com.message));

            CreateMap<CommentDescriptionDTO, CommentDescription>()
                .ForMember(cto => cto.images, c => c.MapFrom(com => Convert.FromBase64String(com.images)))
                .ForMember(cto => cto.message, c => c.MapFrom(com => com.message));

            CreateMap<Comment, CommentFindAll>()
                .ForMember(cto => cto.images, c => c.MapFrom(com => com.images))
                .ForMember(cto => cto.message, c => c.MapFrom(com => com.message));

            CreateMap<CommentDescription, CommentDescriptionFindAll>()
                .ForMember(cto => cto.images, c => c.MapFrom(com => com.images))
                .ForMember(cto => cto.message, c => c.MapFrom(com => com.message));
        }

        // Hàm chuyển từ mảng byte[] sang Base4
        public static string ToBase64(byte[] bytes)
        {
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        // Hàm chuyển từ Base64 sang mảng byte[]
        public static byte[] ToByArr(string text)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(text);
                return bytes;
            }catch(FormatException)
            {
                throw new ArgumentException("Lỗi chuyển đổi");
            }
        }
    }
}
