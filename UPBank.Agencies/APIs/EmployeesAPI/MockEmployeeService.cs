using UPBank.Agencies.APIs.EmployeesAPI.Interface;
using UPBank.Models;

namespace UPBank.Agencies.APIs.EmployeesAPI
{
    public class MockEmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employees;

        public MockEmployeeService()
        {
            _employees = new List<Employee>
            {
                new Employee
                {
                    Cpf = "12345678901",
                    Address = new Address
                    {
                        Zipcode = "11111-111",
                        Street = "Rua 1",
                        Number = 1,
                        Complement = "Casa",
                        Neighborhood = "Bairro 1",
                        City = "Cidade 1",
                        State = "Estado 1"
                    },
                    AddressZipcode = "11111-111",
                    AgencyNumber = "123",
                    Name = "John Doe",
                    BirthDate = new DateTime(1990, 1, 1),
                    Gender = 'M',
                    Manager = false,
                    Phone = "123456789",
                    Register = 1,
                    Salary = 1000,
                    Email = ""
                },
                new Employee
                {
                    Cpf = "12345678902",
                    Address = new Address
                    {
                        Zipcode = "11111-111",
                        Street = "Rua 1",
                        Number = 1,
                        Complement = "Casa",
                        Neighborhood = "Bairro 1",
                        City = "Cidade 1",
                        State = "Estado 1"
                    },
                    AddressZipcode = "11111-111",
                    AgencyNumber = "123",
                    Name = "Jane Doe",
                    BirthDate = new DateTime(1990, 1, 1),
                    Gender = 'F',
                    Manager = true,
                    Phone = "123456789",
                    Register = 2,
                    Salary = 2000,
                    Email = ""
                }
            };
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByAgencyNumber(string number) => await Task.FromResult(_employees.Where(e => e.AgencyNumber == number).ToList());
        
        public Task<Employee> PostManagerEmployee(Employee employee)
        {
            _employees.Add(employee);
            return Task.FromResult(employee);
        }
    }
}