using Localization.AspNetCore.Service.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service.Extensions
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationService(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, AttributeStringLocalizerFactory>();

            services.AddTransient<ILocalizationService, LocalizationService>();
            services.AddTransient<IDictionaryService, DictionaryService>();
            services.AddTransient<IDynamicTextService, DynamicTextService>();
        }
    }
}