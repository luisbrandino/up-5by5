using UPBank.Models;

namespace UPBank.Agencies.APIs.EmployeesAPI.Interface
{
    public interface IEmployeeService
    {
        public Task<List<Employee>> GetEmployeesByAgencyNumber(string number);
    }
}