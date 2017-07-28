﻿using System;
using System.IO;
using System.Text;
using Localization.CoreLibrary.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Localization.CoreLibrary.Util.Impl
{
    public class JsonConfigurationReader
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        private readonly string m_configPath;

        public JsonConfigurationReader(string configPath)
        {
            if (!File.Exists(configPath))
            {
                string errorMsg = string.Format("Configuration file \"{0}\" does not exist or you don't have permisson to read.", configPath);

                Logger.LogError(errorMsg);

                throw new Exception(errorMsg);
            }

            m_configPath = configPath;
        }


        public IConfiguration ReadConfiguration()
        {           
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new ToStringJsonConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            LocalizationConfiguration.Configuration configuration;

            using (Stream stream = new FileStream(m_configPath, FileMode.Open))
            using (StreamReader streamReader = new StreamReader(stream, Encoding.Unicode))
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
            {
                configuration = serializer.Deserialize<LocalizationConfiguration.Configuration>(jsonReader);
            }
            
            return new LocalizationConfiguration(configuration);
        }
    }
}