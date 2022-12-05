using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TimeZoneConverter;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace aspnet_core_dotnet_core.Pages
{
    public class IndexModel : PageModel
    {
        string easternZoneId = "Eastern Standard Time";
        string cetZoneId = "Central European Standard Time";

        // requires using Microsoft.Extensions.Configuration;
        private readonly IConfiguration Configuration;

        public IndexModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async void OnGet()
        {
            var currentTime = DateTime.UtcNow;

            ViewData["UTCDateTime"] = currentTime.ToString();
            ViewData["ESTDateTime"] = null;
            ViewData["CETDateTime"] = null;

            try
            {
                TimeZoneInfo easternZone = TZConvert.GetTimeZoneInfo(easternZoneId);
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
                TimeZoneInfo cetZone = TZConvert.GetTimeZoneInfo(cetZoneId);
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

            try
            {
                var connectionString = (String)Configuration["ConnectionStrings:MyDb"];

                if (string.IsNullOrEmpty(connectionString))
                    throw new ApplicationException("Connection string is empty");

                Trace.WriteLine("Connection string: " + connectionString.Substring(0, 20) + "...");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();

                    SqlCommand cmd = new SqlCommand("SELECT GETDATE() AT TIME ZONE 'UTC' AS UTCDateTime, GETDATE() AT TIME ZONE 'Eastern Standard Time' AS ESTDateTime, GETDATE() AT TIME ZONE 'Central European Standard Time' AS CETDateTime", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            ViewData["UTCDateTimeFromDB"] = reader.GetDateTimeOffset(0);
                            ViewData["ESTDateTimeFromDB"] = reader.GetDateTimeOffset(1);
                            ViewData["CETDateTimeFromDB"] = reader.GetDateTimeOffset(2);
                        } else {
                            ViewData["UTCDateTimeFromDB"] = "No data available in reader";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["UTCDateTimeFromDB"] = ex.Message;
                throw ex;
            }
        }
        public string DoTest()
        {
            return "Index";
        }
    }
}