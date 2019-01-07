﻿using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class DatabaseDictionaryManager : ManagerBase, IDatabaseDictionaryManager
    {
        private readonly IDatabaseDictionaryService m_dbDictionaryService;

        public DatabaseDictionaryManager(
            LocalizationConfiguration configuration, IDatabaseDictionaryService dbDictionaryService, ILogger<DatabaseDictionaryManager> logger = null
        ) : base(configuration, logger)
        {
            m_dbDictionaryService = dbDictionaryService;
        }

        public IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            return m_dbDictionaryService.GetDictionary(cultureInfo, scope);
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            return m_dbDictionaryService.GetPluralizedDictionary(cultureInfo, scope);
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            return m_dbDictionaryService.GetConstantsDictionary(cultureInfo, scope);
        }
    }
}
