using System.Text;
using System;
using UPBank.Agencies.APIs.EmployeesAPI.Interface;
using UPBank.Agencies.APIs.Utils;
using UPBank.Models;
using Newtonsoft.Json;

namespace UPBank.Agencies.APIs.EmployeesAPI
{
    public class EmployeeService : IEmployeeService
    {

        private readonly string _Employee = "https://localhost:7028/api/Employees/";

        public async Task<IEnumerable<Employee>> GetEmployeesByAgencyNumber(string agencyNumber)
        {
           var url = _Employee + "agency/" + agencyNumber;

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            return await ApiUtils<IEnumerable<Employee>>.GetObjectFromResponse(response) ?? new List<Employee>();
        }

        public async Task<Employee> PostManagerEmployee(Employee employee)
        {
            var url = _Employee + "hire";

            using var client = new HttpClient();
            string jsonContent = JsonConvert.SerializeObject(employee);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            response.EnsureSuccessStatusCode();
            string strResponse = response.Content.ReadAsStringAsync().Result;

            var createdEmployee = JsonConvert.DeserializeObject<Employee>(strResponse);

            return createdEmployee;
        }
    }
}