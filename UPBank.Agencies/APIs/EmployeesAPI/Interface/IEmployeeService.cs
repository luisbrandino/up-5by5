using UPBank.Models;

namespace UPBank.Agencies.APIs.EmployeesAPI.Interface
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetEmployeesByAgencyNumber(string number);

        public Task<Employee> PostManagerEmployee(Employee employee);
    }
}