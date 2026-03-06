using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace DotNetBili.Common
{
    public class AppSettings
    {
        public static IConfiguration Configuration { get; set; } = null!;

        public AppSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public AppSettings(string contentPath)
        {
            string Path = "appsettings.json";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(contentPath)
                .Add(new JsonConfigurationSource
                {
                    Path = Path,
                    Optional = false,
                    ReloadOnChange = true
                })
                .Build();
        }

        /// <summary>
        /// 封装要操作的字段
        /// app("redis","connection")
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public static string app(params string[] section)
        {
            try
            {
                if (section.Any())
                {
                    return Configuration[string.Join(":", section)] ?? "";
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// 递归获取配置消息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <returns></returns>
        public static List<T> app<T>(params string[] section)
        {
            List<T> list = new List<T>();

            Configuration.Bind(string.Join(":", section), list);

            return list;
        }

        /// <summary>
        /// 根据路径获取配置消息 Configuration["redis:connection"]
        /// </summary>
        /// <param name="sectionPath"></param>
        /// <returns></returns>
        public static string GetValue(string sectionPath)
        {
            try
            {
                return Configuration[sectionPath] ?? "";
            }
            catch (Exception)
            {
            }
            return "";
        }
    }
}
