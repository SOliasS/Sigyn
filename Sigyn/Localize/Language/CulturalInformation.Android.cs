using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

//[assembly: Xamarin.Forms.Dependency(typeof(Sigyn.Localize.Language.CulturalInformation))]
namespace Sigyn.Localize.Language
{
    public class CulturalInformation: ICulturalInformation
    {        

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Console.WriteLine("CurrentCulture set: " + ci.Name);
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            var androidLocale = Java.Util.Locale.Default;
            netLanguage = AndroidToDotnetLanguage(androidLocale.ToString().Replace("_", "-"));

            // this gets called a lot - try/catch can be expensive so consider caching or something
            System.Globalization.CultureInfo ci = null;
            try
            {
                ci = new System.Globalization.CultureInfo(netLanguage);
            }

            catch (Exception ex)
            {
                // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
                // fallback to first characters, in this case "en"
                try
                {
                    var fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
                    ci = new System.Globalization.CultureInfo(fallback);
                }
                catch (Exception ex2)
                {
                    Crashes.TrackError(ex2);
                }
            }

            return ci;
        }

        string AndroidToDotnetLanguage(string androidLanguage)
        {
            var netLanguage = androidLanguage;

            //certain languages need to be converted to CultureInfo equivalent
            switch (androidLanguage)
            {
                case "in-ID":
                    netLanguage = "id-ID";
                    break;
                case "gsw-CH":
                    netLanguage = "de-CH";
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
                case "gsw":
                    netLanguage = "de-CH";
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
