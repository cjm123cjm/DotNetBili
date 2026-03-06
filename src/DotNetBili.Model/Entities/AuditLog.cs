using SqlSugar;

namespace DotNetBili.Model.Entities
{
    [Tenant("log")]
    [SplitTable(SplitType.Month)]
    [SugarTable($@"{nameof(AuditLog)}_{{year}}{{month}}{{day}}")]
    public class AuditLog
    {
        public int Id { get; set; }

        [SplitField]
        public DateTime CreateTime { get; set; }
        public string UserName { get; set; }
    }
}
