using AutoMapper;
using DotNetBili.Extension.Profiles;

namespace DotNetBili.Extension.ServicesExtension
{
    /// <summary>
    /// 静态全局 AutoMapper 配置文件
    /// </summary>
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });
            return config;
        }
    }
}
