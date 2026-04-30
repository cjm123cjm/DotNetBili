using AutoMapper;
using DotNetBili.Model.Dto.Account;
using DotNetBili.Model.Entities;

namespace DotNetBili.Extension.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInfo, UserInfoDto>().ReverseMap();
        }
    }
}
