using Newtonsoft.Json;
using UPBank.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UPBank.Employees.Service
{
    public class EmployeeService
    {
        private readonly string _Agency = ""; // Endpoint of Agency API

        public async Task<Agency?> GetAgencyAsync(string AgencyNumber)
        {
            Agency? agency = null;
            string url = _Agency + AgencyNumber;

            try
            {
                using HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    agency = JsonConvert.DeserializeObject<Agency>(json);
                }
            }
            catch (Exception e) { Console.WriteLine(e); }

            return agency;
        }

    }
}
