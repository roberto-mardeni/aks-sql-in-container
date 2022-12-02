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
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(easternZoneId);
            TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById(cetZoneId);

            var currentTime = DateTime.UtcNow;

            ViewData["UTCDateTime"] = currentTime.ToString();
            ViewData["ESTDateTime"] = TimeZoneInfo.ConvertTime(currentTime, easternZone).ToString();
            ViewData["CETDateTime"] = TimeZoneInfo.ConvertTime(currentTime, cetZone).ToString();
        }
        public string DoTest()
        {
            return "Index";
        }
    }
}