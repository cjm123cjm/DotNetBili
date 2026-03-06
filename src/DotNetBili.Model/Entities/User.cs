using DotNetBili.Model.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBili.Model.Entities
{
    [MultiTenant(TenantTypeEnum.Tables)]
    public class User
    {
    }
}
