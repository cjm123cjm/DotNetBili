using DotNetBili.Common;
using DotNetBili.Common.DB;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace DotNetBili.Extension.ServicesExtension
{
    public static class SqlsugarSetup
    {
        public static void AddSqlsugerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //默认添加主数据库的链接
            if (!string.IsNullOrWhiteSpace(AppSettings.app("MainDB")))
            {
                MainDb.CurrentDbConId = AppSettings.app("MainDB");
            }

            //配置WORKID
            var workerId = AppSettings.app("SnowFlakeWorkerId"); // 需要在 appsettings.json 中配置
            if (string.IsNullOrWhiteSpace(workerId))
            {
                // 方式2：自动生成（基于机器特征或随机数）
                workerId = GetAutoWorkerId();
            }
            SnowFlakeSingle.WorkId = int.Parse(workerId);

            BaseDBConfig.MutiConnectionString.allDbs.ForEach(m =>
            {
                var config = new ConnectionConfig
                {
                    ConfigId = m.ConnId.ToLower(),
                    ConnectionString = m.Connection,
                    DbType = (DbType)m.DbType,
                    IsAutoCloseConnection = true,
                    MoreSettings = new ConnMoreSettings
                    {
                        IsAutoRemoveDataCache = true,
                        SqlServerCodeFirstNvarchar = true
                    },
                    InitKeyType = InitKeyType.Attribute,
                };
                if (SqlSugarConst.LogConfigId.ToLower().Equals(m.ConnId.ToLower()))
                {
                    BaseDBConfig.LogConfig = config;
                }
                else
                {
                    BaseDBConfig.ValidConfig.Add(config);
                }

                BaseDBConfig.AllConfigs.Add(config);
            });

            if (BaseDBConfig.LogConfig is null)
            {
                throw new ApplicationException("未找到日志数据库链接，请检查配置文件");
            }

            services.AddSingleton<ISqlSugarClient>(o =>
            {
                return new SqlSugarScope(BaseDBConfig.AllConfigs, db =>
                {
                    BaseDBConfig.ValidConfig.ForEach(config =>
                    {
                        var dbProvider = db.GetConnectionScope((string)config.ConfigId);
                        // 配置实体数据权限（多租户）
                        RepositorySetting.SetTenantEntityFilter(dbProvider);
                    });
                });
            });
        }

        // 自动生成 WorkerId 的辅助方法
        private static string GetAutoWorkerId()
        {
            // 方法A：使用随机数（范围 0-1023）
            var random = new Random();
            var workerId = random.Next(0, 1024);

            // 方法B：根据机器特征生成（如获取本机IP的最后一段）
            // var hostName = System.Net.Dns.GetHostName();
            // var ip = System.Net.Dns.GetHostEntry(hostName).AddressList.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            // var workerId = ip?.ToString().Split('.').LastOrDefault() ?? "0";

            return workerId.ToString();
        }
    }
}
