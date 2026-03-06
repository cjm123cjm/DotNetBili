using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBili.Admin.Extensions
{
    public class AutofacPropertityModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();
        }
    }
}
