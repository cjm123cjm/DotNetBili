using DotNetBili.IService.Base;
using DotNetBili.Model.Dto;
using DotNetBili.Model.Dto.Account;
using DotNetBili.Model.Entities;

namespace DotNetBili.IService
{
    public interface IUserInfoService : IBaseServices<UserInfo>
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerInput"></param>
        /// <returns></returns>
        Task RegisterAsync(RegisterInput registerInput);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        Task<LoginResponseDto> LoginAsync(LoginInput loginInput);
    }
}
