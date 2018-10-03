﻿using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service
{
    public class DictionaryService : ServiceBase, IDictionaryService
    {
        private readonly IAutoDictionaryManager m_dictionaryManager;

        public DictionaryService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            m_dictionaryManager = Localization.CoreLibrary.Localization.Dictionary;
        }

        public Dictionary<string, LocalizedString> GetDictionary(string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_dictionaryManager.GetDictionary(translationSource, requestCulture, scope);
        }

        public Dictionary<string, PluralizedString> GetPluralizedDictionary(string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_dictionaryManager.GetPluralizedDictionary(translationSource, requestCulture, scope);
        }

        public Dictionary<string, LocalizedString> GetConstantsDictionary(string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_dictionaryManager.GetConstantsDictionary(translationSource, requestCulture, scope);
        }

        /// <summary>
        /// Gets and returns Culture from request httpContext culture cookie.
        /// </summary>
        /// <returns> Culture from request httpContext culture cookie.</returns>
        protected CultureInfo RequestCulture()
        {
            HttpRequest request = HttpContextAccessor.HttpContext.Request;

            string cultureCookie = request.Cookies[ServiceBase.CultureCookieName];
            if (cultureCookie == null)
            {
                cultureCookie = m_dictionaryManager.DefaultCulture().Name;
            }

            return new CultureInfo(cultureCookie);
        }

        
    }
}