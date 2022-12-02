using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace aspnet_core_dotnet_core.Pages
{
    public class IndexModel : PageModel
    {
        string easternZoneId = "Eastern Standard Time";
        string cetZoneId = "Central European Standard Time";

        public void OnGet()
        {
            var currentTime = DateTime.UtcNow;

            ViewData["UTCDateTime"] = currentTime.ToString();
            ViewData["ESTDateTime"] = null;
            ViewData["CETDateTime"] = null;

            try
            {
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(easternZoneId);
                ViewData["ESTDateTime"] = TimeZoneInfo.ConvertTime(currentTime, easternZone).ToString();
            }
            catch (TimeZoneNotFoundException)
            {
                ViewData["ESTDateTime"] = "The registry does not define the Eastern Standard Time zone.";
            }
            catch (InvalidTimeZoneException)
            {
                ViewData["ESTDateTime"] = "Registry data on the Eastern Standard Time zone has been corrupted.";
            }

            try
            {
                TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById(cetZoneId);
                ViewData["CETDateTime"] = TimeZoneInfo.ConvertTime(currentTime, cetZone).ToString();
            }
            catch (TimeZoneNotFoundException)
            {
                ViewData["CETDateTime"] = "The registry does not define the Central European Standard Time zone.";
            }
            catch (InvalidTimeZoneException)
            {
                ViewData["CETDateTime"] = "Registry data on the Central European Standard Time zone has been corrupted.";
            }
        }
        public string DoTest()
        {
            return "Index";
        }
    }
}