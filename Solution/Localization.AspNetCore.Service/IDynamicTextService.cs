using Localization.CoreLibrary.Entity;

namespace Localization.AspNetCore.Service
{
    public interface IDynamicTextService
    {
        DynamicText GetDynamicText(string name, string scope);
        DynamicText SaveDynamicText(DynamicText dynamicText);
    }
}