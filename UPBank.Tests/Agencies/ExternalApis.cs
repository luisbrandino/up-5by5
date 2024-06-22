using UPBank.Models;

namespace UPBank.Tests.Agencies
{
    public class ExternalApis
    {
        private List<Employee> _employees;
        private List<Address> _addresses;

        public ExternalApis() 
        {
            _employees = new List<Employee>
            {
                new Employee
                {
                    Name = "Funcionário 1",
                    Cpf = "12345678901",
                    AgencyNumber = "123"
                },
                new Employee
                {
                    Name = "Funcionário 2",
                    Cpf = "12345678902",
                    AgencyNumber = "123"
                },
                new Employee
                {
                    Name = "Funcionário 3",
                    Cpf = "12345678903",
                    AgencyNumber = "456"
                },
                new Employee
                {
                    Name = "Funcionário 4",
                    Cpf = "12345678904",
                    AgencyNumber = "456"
                }
            };

            _addresses = new List<Address>
            {
                new Address
                {
                    Zipcode = "12345678",
                    Street = "Rua 1",
                    Number = 123,
                    Complement = "Casa",
                    Neighborhood = "Bairro 1",
                    City = "Cidade 1",
                    State = "Estado 1"
                },
                new Address
                {
                    Zipcode = "12365678",
                    Street = "Rua 2",
                    Number = 456,
                    Complement = "Casa",
                    Neighborhood = "Bairro 2",
                    City = "Cidade 2",
                    State = "Estado 2"
                }
            };
        }

        public Address GetAddressFromExternalApiByZipcode(string zipcode)
        {
            return _addresses.Find(address => address.Zipcode == zipcode);
        }

        public List<Employee> GetEmployeeFromExternalApiByAgency(string agencyNumber)
        {
            return _employees.FindAll(employee => employee.AgencyNumber == agencyNumber);
        }
    }
}