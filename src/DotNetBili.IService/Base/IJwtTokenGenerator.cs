using DotNetBili.Model.Entities;

namespace DotNetBili.IService.Base
{
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateToken(UserInfo user);
        string GenerateToken(string userName);
    }
}
