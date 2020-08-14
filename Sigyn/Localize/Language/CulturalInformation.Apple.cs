using Foundation;
using System.Globalization;
using System.Threading;

//[assembly: Xamarin.Forms.Dependency(typeof(Sigyn.Localize.Language.CulturalInformation))]
namespace Sigyn.Localize.Language
{

    public class CulturalInformation
    {
        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];

                netLanguage = iOSToDotnetLanguage(pref);
            }

            // this gets called a lot - try/catch can be expensive so consider caching or something
            CultureInfo ci = null;
            try
            {
                ci = new CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException e1)
            {
                // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
                // fallback to first characters, in this case "en"
                try
                {
                    var fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
                    ci = new CultureInfo(fallback);
                }
                catch (CultureNotFoundException e2)
                {
                    // iOS language not valid .NET culture, falling back to English
                    ci = new CultureInfo("en");
                }
            }

            return ci;
        }

        string iOSToDotnetLanguage(string iOSLanguage)
        {
            // .NET cultures don't support underscores
            var netLanguage = iOSLanguage.Replace("_", "-");

            // certain languages need to be converted to CultureInfo equivalent
            switch (iOSLanguage)
            {
                case "gsw-CH":  // "Schwiizertüütsch (Swiss German)" not supported .NET culture
                    netLanguage = "de-CH"; // closest supported
                    break;
                case "in-ID":  // "Indonesian (Indonesia)" has different code in  .NET 
                    netLanguage = "id-ID"; // correct code for .NET
                    break;
                case "en-DE":
                    netLanguage = "de-DE";
                    break;
                case "en-US":
                    netLanguage = "en-US";
                    break;
            }

            return netLanguage;
        }
        string ToDotnetFallbackLanguage(PlatformCulture platCulture)
        {
            var netLanguage = platCulture.LanguageCode; // use the first part of the identifier (two chars, usually);

            switch (platCulture.LanguageCode)
            {
                // force different 'fallback' behavior for some language codes
                case "gsw":
                    netLanguage = "de-CH"; // equivalent to German (Switzerland) for this app
                    break;
                case "de":
                    netLanguage = "de-DE";
                    break;
                case "en":
                    netLanguage = "en-US";
                    break;
                default:
                    netLanguage = "en-US";
                    break;
            }

            return netLanguage;
        }
    }
}