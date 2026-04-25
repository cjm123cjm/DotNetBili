namespace DotNetBili.Common.Option
{
    /// <summary>
    /// jwt配置
    /// </summary>
    public class JwtOptions : IConfigurableOptions
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; } = null!;
        /// <summary>
        /// 发行方
        /// </summary>
        public string Issuer { get; set; } = null!;
        /// <summary>
        /// 授权方
        /// </summary>
        public string Audience { get; set; } = null!;
        /// <summary>
        /// 过期时间/min
        /// </summary>
        public int Expires { get; set; }
    }
}
