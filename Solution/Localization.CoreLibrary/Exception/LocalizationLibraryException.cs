﻿using Localization.CoreLibrary.Logging;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Exception
{
    public class LocalizationLibraryException : System.Exception
    {
        public LocalizationLibraryException()
        {
        }

        public LocalizationLibraryException(string message) : base(message)
        {
        }

        public LocalizationLibraryException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}