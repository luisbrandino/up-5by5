using Newtonsoft.Json;
using UPBank.Agencies.APIs.EmployeesAPI.Interface;
using UPBank.Models;

namespace UPBank.Agencies.APIs.EmployeesAPI
{
    public class EmployeeService : IEmployeeService
    {

        private readonly string _Employee = "https://localhost:####/api/Employees/";

        public async Task<List<Employee>> GetEmployeesByAgencyNumber(string agencyNumber)
        {
            var url = _Employee + agencyNumber;
            List<Employee> employees = new List<Employee>();

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(json))
                    employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(json).ToList();
            }

            return employees;
        }
    }
}