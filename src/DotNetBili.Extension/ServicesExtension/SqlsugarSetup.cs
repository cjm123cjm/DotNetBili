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
                    InitKeyType = InitKeyType.Attribute
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

            if(BaseDBConfig.LogConfig is null)
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
    }
}
