using Autofac;
using Autofac.Extras.DynamicProxy;
using DotNetBili.IService;
using DotNetBili.IService.Base;
using DotNetBili.Repository.Base;
using DotNetBili.Repository.UnitOfWorks;
using DotNetBili.Service.Base;
using System.Reflection;

namespace DotNetBili.Extension.ServicesExtension
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            builder.RegisterType<JwtTokenGenerator>().As<IJwtTokenGenerator>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            var servicesDllPath = Path.Combine(basePath, "DotNetBili.Service.dll");
            var repositoryDllPath = Path.Combine(basePath, "DotNetBili.Repository.dll");

            List<Type> aopTypes = new List<Type> { typeof(ServiceAOP), typeof(TranAOP) };
            builder.RegisterType<ServiceAOP>();
            builder.RegisterType<TranAOP>();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();//仓储
            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseServices<>))
                    .InstancePerDependency()
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(aopTypes.ToArray());//服务

            //获取 Service.dll 程序集并注册 生命周期为 InstancePerDependency，并启用属性注入和接口拦截器，拦截器类型为 ServiceAOP
            var serviceAssembly = Assembly.LoadFrom(servicesDllPath);
            builder.RegisterAssemblyTypes(serviceAssembly)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .EnableInterfaceInterceptors()
                .InterceptedBy(aopTypes.ToArray());

            //获取 Repository.dll 程序集并注册 生命周期为 InstancePerDependency，并启用属性注入
            var repositoryAssembly = Assembly.LoadFrom(repositoryDllPath);
            builder.RegisterAssemblyTypes(repositoryAssembly)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired();

            builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}
