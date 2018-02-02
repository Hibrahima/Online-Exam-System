using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExamSytem.Helpers;

namespace OnlineExamSytem.Controllers
{
    public class LanguageController : BaseController
    {
        public ActionResult SetCulture(string culture, string returnUrl)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            RouteData.Values["culture"] = culture;
            if (returnUrl != "/")
            {
                if (returnUrl.Contains("tr"))  // set culture
                    returnUrl = returnUrl.Replace("tr", culture);
                else if (returnUrl.Contains("fr"))  // set culture
                    returnUrl = returnUrl.Replace("fr", culture);
                else
                    returnUrl = returnUrl.Replace("en-us", culture);
            }
            else
            {
                returnUrl = returnUrl + culture;
            }

            Console.WriteLine(returnUrl);

            return Redirect(returnUrl);
        }

    }
}