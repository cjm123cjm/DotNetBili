using SqlSugar;

namespace DotNetBili.Model.Tenant
{
    public interface ITenantEntity
    {
        /// <summary>
        /// 租户id
        /// </summary>
        [SugarColumn(DefaultValue = "0")]
        public long TenantId { get; set; }
    }
}
