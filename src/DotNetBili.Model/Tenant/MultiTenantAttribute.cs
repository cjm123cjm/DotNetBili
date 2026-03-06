namespace DotNetBili.Model.Tenant
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MultiTenantAttribute : Attribute
    {
        public MultiTenantAttribute()
        {
                
        }
        public MultiTenantAttribute(TenantTypeEnum typeEnum)
        {
            TypeEnum = typeEnum;
        }

        public TenantTypeEnum TypeEnum { get; }
    }
}
