﻿using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary.Factory;
using Scalesoft.Localization.Core.Exception;
using Scalesoft.Localization.Core.Manager.Impl;
using Scalesoft.Localization.Core.Resolver;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.Core.Tests.Manager
{
    [TestClass]
    public class DictionaryManagerTest
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void CultureSupportTest()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es")
                },
            };

            var dictionaryManager = new FileDictionaryManager(
                localizationConfiguration, JsonDictionaryFactory.FactoryInstance
            );

            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("cs")));
            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("en")));
            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("es")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("zh")));
            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("en-gb")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(null));
        }

        [TestMethod]
        public void ScopeAliasSupportTest()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es")
                },
                AutoLoadResources = true
            };

            var dictionaryManager = new FileDictionaryManager(
                localizationConfiguration, JsonDictionaryFactory.FactoryInstance
            );
            var dictionaryScope = dictionaryManager.GetLocalizationDictionary(new CultureInfo("cs"), "global");
            var dictionaryAlias1 = dictionaryManager.GetLocalizationDictionary(new CultureInfo("cs"), "personalScope1");
            var dictionaryAlias2 = dictionaryManager.GetLocalizationDictionary(new CultureInfo("cs"), "personalScope2");

            Assert.AreSame(dictionaryScope, dictionaryAlias1);
            Assert.AreSame(dictionaryScope, dictionaryAlias2);
        }

        [TestMethod]
        public void TreeTest()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "LocalizationTree",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("en-US"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("en-CA"),
                    new CultureInfo("es-MX"),
                    new CultureInfo("es-US"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key,
                AutoLoadResources = true
            };

            var dictionaryManager = new FileDictionaryManager(
                localizationConfiguration, JsonDictionaryFactory.FactoryInstance
            );

            var fallbackCultureResolver = new FallbackCultureResolver(localizationConfiguration);
            var fileLocalizationManager = new FileLocalizationManager(
                localizationConfiguration, dictionaryManager, fallbackCultureResolver
            );

            var s1 = fileLocalizationManager.Translate("text-1-odst", new CultureInfo("cs"));
            Assert.AreEqual("global cs [text-1-odst]", s1);

            var s2 = fileLocalizationManager.Translate("extra-cs-key", new CultureInfo("en-MX"));
            Assert.AreEqual("extra string in CS culture", s2);

            var s3 = fileLocalizationManager.Translate("extra-cs-key", new CultureInfo("es-MX"));
            Assert.AreEqual("extra string in CS culture", s3);

            const string nopeKey = "nope-key";
            var sNope = fileLocalizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            Assert.AreEqual(nopeKey, sNope);

            localizationConfiguration.TranslateFallbackMode = LocTranslateFallbackMode.EmptyString;

            var sNope2 = fileLocalizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            Assert.AreEqual("", sNope2);

            localizationConfiguration.TranslateFallbackMode = LocTranslateFallbackMode.Exception;

            var exceptionThrown = false;
            try
            {
                var sNope3 = fileLocalizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            }
            catch (TranslateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}