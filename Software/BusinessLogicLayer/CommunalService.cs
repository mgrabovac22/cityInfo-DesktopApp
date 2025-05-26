using DataAccessLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    //Marin Grabovac
    public class CommunalService
    {

        public string AddPeriodLocation(string period, string location, string locationArea, string day, string wasteType)
        {
            using (var repo = new CommunalRepository())
            {

                var periodWaste = $"{period} u {day} za {wasteType}";

                PeriodLocation pl = new PeriodLocation
                {
                    Period = periodWaste,
                    Location = location,
                    Location_area = locationArea,
                    ID_Institution = 1
                };

                repo.AddPL(pl);

                return $"Nova lokacija dodana! Odvoz smeća u ulici {location} je {periodWaste}.";
            }
        }

        public PeriodLocation GetLocationInfoByAddress(string address)
        {
            using (var repo = new CommunalRepository())
            {
                return repo.GetOneByLocation(address);
            }
        }

        public async Task<dynamic> GetAddressFromApiAsync(double lat, double lng)
        {
            string url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lng}&addressdetails=1";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "YourAppName/1.0 (your.email@example.com)");
                    var response = await client.GetStringAsync(url);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
