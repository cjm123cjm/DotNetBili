using DotNetBili.Common.Option;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetBili.Extension.ServicesExtension
{
    public static class AllOptionRegister
    {
        public static void AddAllOptionRegister(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            foreach (var optionType in typeof(ConfigurableOptions).Assembly.GetTypes().Where(t => !t.IsInterface && typeof(IConfigurableOptions).IsAssignableFrom(t)))
            {
                services.AddConfigurableOptions(optionType);
            }
        }
    }
}
