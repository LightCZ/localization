﻿using System.Collections.Generic;
using System.Globalization;

namespace Localization
{
    public interface ILocalizationResourceManager
    {
        string GetString(string text, CultureInfo currentCultureInfo);

        IDictionary<string, string> GetDictionary(CultureInfo cultureInfo);
    }
}