using System.Security.Claims;

namespace DotNetBili.Common.HttpContextUser
{
    public interface IUser
    {
        string Name { get; }
        long ID { get; }
        long TenantId { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
        List<string> GetClaimValueByType(string ClaimType);

        string GetToken();
        List<string> GetUserInfoFromToken(string ClaimType);

        /// <summary>
        /// 获取客户端IP地址（支持反向代理）
        /// </summary>
        /// <returns></returns>
        string GetClientIp();

        /// <summary>
        /// 获取客户端IP地址（IPv4格式，处理IPv6映射）
        /// </summary>
        /// <returns></returns>
        string GetClientIpv4();
    }
}
