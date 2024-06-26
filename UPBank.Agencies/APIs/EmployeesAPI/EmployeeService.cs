using UPBank.Agencies.APIs.EmployeesAPI.Interface;
using UPBank.Agencies.APIs.Utils;
using UPBank.Models;

namespace UPBank.Agencies.APIs.EmployeesAPI
{
    public class EmployeeService : IEmployeeService
    {

        private readonly string _Employee = "https://localhost:7028/api/Employees/";

        public async Task<IEnumerable<Employee>> GetEmployeesByAgencyNumber(string agencyNumber)
        {
            var url = _Employee + agencyNumber;

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            return await ApiUtils<IEnumerable<Employee>>.GetObjectFromResponse(response) ?? new List<Employee>();
        }

        public async Task<Employee> PostManagerEmployee(Employee employee)
        {
            var url = _Employee;

            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync(url, employee);

            return await ApiUtils<Employee>.GetObjectFromResponse(response) ?? new Employee();
        }
    }
}