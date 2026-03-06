namespace DotNetBili.Common.Option
{
    /// <summary>
    /// redis配置
    /// </summary>
    public class RedisOptions : IConfigurableOptions
    {
        /// <summary>
        /// 是否启用Redis缓存
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// redis链接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string InstanceName { get; set; }
    }
}
