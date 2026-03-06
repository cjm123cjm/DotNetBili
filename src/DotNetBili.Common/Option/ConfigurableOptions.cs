using DotNetBili.Common.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DotNetBili.Common.Option
{
    public static class ConfigurableOptions
    {
        /// <summary>
        /// 获取配置路
        /// </summary>
        /// <param name="optionType">选项类型</param>
        /// <returns></returns>
        public static string GetConfigurationPath(Type optionType)
        {
            var endPath = new[] { "Option", "Options" };
            var configrationPath = optionType.Name;
            foreach (var item in endPath)
            {
                if (configrationPath.EndsWith(item))
                {
                    return configrationPath[..^item.Length];
                }
            }

            return configrationPath;
        }

        /// <summary>
        /// 添加选项配置
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurableOptions<TOptions>(this IServiceCollection services) where TOptions : class, IConfigurableOptions
        {
            Type optionsType = typeof(TOptions);
            string path = GetConfigurationPath(optionsType);

            services.Configure<TOptions>(App.Configuration.GetSection(path));

            return services;
        }

        public static IServiceCollection AddConfigurableOptions(this IServiceCollection services, Type type)
        {
            string path = GetConfigurationPath(type);
            var config = App.Configuration.GetSection(path);

            Type iOptionsChangeTokenSource = typeof(IOptionsChangeTokenSource<>);
            Type iConfigureOptions = typeof(IConfigureOptions<>);
            Type configurationChangeTokenSource = typeof(ConfigurationChangeTokenSource<>);
            Type namedConfigureFromConfigurationOptions = typeof(NamedConfigureFromConfigurationOptions<>);
            iOptionsChangeTokenSource = iOptionsChangeTokenSource.MakeGenericType(type);
            iConfigureOptions = iConfigureOptions.MakeGenericType(type);
            configurationChangeTokenSource = configurationChangeTokenSource.MakeGenericType(type);
            namedConfigureFromConfigurationOptions = namedConfigureFromConfigurationOptions.MakeGenericType(type);

            services.AddOptions();
            services.AddSingleton(iOptionsChangeTokenSource,
                Activator.CreateInstance(configurationChangeTokenSource, Options.DefaultName, config) ?? throw new InvalidOperationException());
            return services.AddSingleton(iConfigureOptions,
                Activator.CreateInstance(namedConfigureFromConfigurationOptions, Options.DefaultName, config) ?? throw new InvalidOperationException());
        }
    }
}
