using System;
using System.Collections.Generic;
using System.Text;

namespace Sigyn.Localize.Language
{   
    /// <summary>
     ///     Helper class for splitting locales like
     ///     iOS: ms_MY, gsw_CH
     ///     Android: in-ID
     ///     into parts so we can create a .NET culture (or fallback culture)
     /// </summary>
    public class PlatformCulture
    {
        #region Properties

        public string LanguageCode
        {
            get;
            private set;
        }

        public string LocaleCode
        {
            get;
            private set;
        }

        public string PlatformString
        {
            get;
            private set;
        }

        #endregion Properties

        #region Constructors

        //var t = Resx.BaseLanguage.Greeting;
        public PlatformCulture(string platformCultureString)
        {
            if (string.IsNullOrEmpty(platformCultureString))
            {
                throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));
            }

            PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
            var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);

            if (dashIndex > 0)
            {
                var parts = PlatformString.Split('-');
                LanguageCode = parts[0];
                LocaleCode = parts[1];
            }
            else
            {
                LanguageCode = PlatformString;
                LocaleCode = "";
            }
        }

        #endregion Constructors

        #region Methods

        public override string ToString()
        {
            return PlatformString;
        }

        #endregion Methods
    }
}
