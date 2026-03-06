using DotNetBili.Common.Core;
using DotNetBili.Model.Tenant;
using SqlSugar;

namespace DotNetBili.Common.DB
{
    public class RepositorySetting
    {
        public static void SetTenantEntityFilter(SqlSugarScopeProvider db)
        {
            if (App.User is not { ID: > 0, TenantId: > 0 })
            {
                return;
            }

            //多租户 单表字段
            db.QueryFilter.AddTableFilter<ITenantEntity>(it => it.TenantId == App.User.TenantId || it.TenantId == 0);

            //多租户 多表
            db.SetTenantTable(App.User.TenantId.ToString());
        }

        /// <summary>
        /// 获取所有的实体类（包含多租户和非多租户的）
        /// </summary>
        private static readonly Lazy<IEnumerable<Type>> AllEntitys = new(() =>
        {
            return typeof(MultiTenantAttribute).Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(it => it.FullName != null && it.FullName.StartsWith("DotNetBili.Model.Entitis"));
        });
        public static IEnumerable<Type> Entitys => AllEntitys.Value;
    }
}
