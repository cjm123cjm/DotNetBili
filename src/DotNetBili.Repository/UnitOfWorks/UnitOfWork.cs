using Microsoft.Extensions.Logging;
using SqlSugar;

namespace DotNetBili.Repository.UnitOfWorks
{
    public class UnitOfWork : IDisposable
    {
        public ILogger Logger { get; set; }
        public ISqlSugarClient Db { get; internal set; }
        public ITenant Tenant { get; internal set; }
        public bool IsTran { get; internal set; }
        public bool IsCommit { get; internal set; }
        public bool IsClose { get; internal set; }

        public void Dispose()
        {
            if (IsTran && !IsCommit)
            {
                Logger.LogDebug("UnitOfWork RollbackTran");
                Tenant.RollbackTran();
            }

            if (Db.Ado.Transaction != null || IsClose)
                return;
            Db.Close();
        }

        /// <summary>
        /// 事务是否提交
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            if (IsTran && !IsCommit)
            {
                Logger.LogDebug("UnitOfWork Commit");
                Tenant.CommitTran();
                IsCommit = true;
            }

            if (Db.Ado.Transaction == null && !IsClose)
            {
                Db.Dispose();
                IsClose = true;
            }

            return IsCommit;
        }
    }
}
