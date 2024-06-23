using Dapper;
using Microsoft.Data.SqlClient;
using UPBank.Models;
using UPBank.Models.Utils;
using static System.Net.WebRequestMethods;

namespace UPBank.Customers.Repositories
{
    public class CustomerRepository
    {
        private readonly string _conn;
        private readonly string _addressUri;


        public CustomerRepository()
        {
            _conn = "Data Source=127.0.0.1; Initial Catalog=DBCustumersAPI; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
            _addressUri = "https://localhost:7004";
        }

        public async Task<List<Customer>> GetAll()
        {
            var list = new List<Customer>();
            using var connection = new SqlConnection(_conn);
            connection.Open();

            var queryResult = connection.Query<dynamic>(Customer.SelectAllCustomers).ToList();

            var t1 = ApiConsume<List<Address>>.Get(_addressUri, "/api/addresses");

            var addressResult = t1.Result;

            if (addressResult == null)
                return null;

            foreach (dynamic row in queryResult)
            {
                Customer costumer = new()
                {
                    Cpf = FormatCpf(row.Cpf),
                    Name = row.Name,
                    BirthDate = row.BirthDate,
                    Gender = Convert.ToChar(row.Gender),
                    Salary = Convert.ToDouble(row.Salary),
                    Phone = FormatPhone(row.Phone),
                    Email = row.Email,
                    Address = addressResult.FirstOrDefault(a => a.Zipcode == row.Address),
                    Restriction = row.Restriction,
                };

                list.Add(costumer);
            }
            return list;
        }


        public async Task<List<Customer>> GetAllDeleted()
        {
            var list = new List<Customer>();
            using var connection = new SqlConnection(_conn);
            connection.Open();

            var t1 = ApiConsume<List<Address>>.Get(_addressUri, "/api/addresses");

            var addressResult = t1.Result;

            if (addressResult == null)
                return null;

            foreach (dynamic row in connection.Query<dynamic>(Customer.SelectAllDeletedCustomers).ToList())
            {
                Customer costumer = new()
                {
                    Cpf = FormatCpf(row.Cpf),
                    Name = row.Name,
                    BirthDate = row.BirthDate,
                    Gender = Convert.ToChar(row.Gender),
                    Salary = Convert.ToDouble(row.Salary),
                    Phone = FormatPhone(row.Phone),
                    Email = row.Email,
                    Address = addressResult.FirstOrDefault(a => a.Zipcode == row.Address),
                    Restriction = row.Restriction
                };

                list.Add(costumer);
            }
            return list;
        }
        public async Task<Customer> GetByCPF(string cpf)
        {
            var list = new List<Customer>();
            using var connection = new SqlConnection(_conn);
            connection.Open();

            dynamic? row = connection.Query<dynamic>(Customer.SelectCustomerByCpf, new { Cpf = cpf }).FirstOrDefault();

            if (row == null)
                return null;

            var t1 = ApiConsume<List<Address>>.Get(_addressUri, "/api/addresses");

            var addressResult = t1.Result;

            if (addressResult == null)
                return null;

            Customer custumer = new()
            {
                Cpf = FormatCpf(row.Cpf),
                Name = row.Name,
                BirthDate = row.BirthDate,
                Gender = Convert.ToChar(row.Gender),
                Salary = Convert.ToDouble(row.Salary),
                Phone = FormatPhone(row.Phone),
                Email = row.Email,
                Address = addressResult.FirstOrDefault(a => a.Zipcode == row.Address),
                Restriction = row.Restriction
            };

            return custumer;
        }

        public async Task<Customer> PostCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();

                connection.Execute(Customer.InsertCustomer, new
                {
                    Cpf = customer.Cpf,
                    Name = customer.Name,
                    BirthDate = customer.BirthDate,
                    Gender = customer.Gender,
                    Salary = customer.Salary,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    Address = customer.Address,
                    Restriction = customer.Restriction
                });


                return customer;
            }
        }
        public async Task<Customer> DeleteCustomer(string cpf)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();

                var costumer = GetByCPF(cpf).Result;
                if (costumer == null)
                {
                    return null;
                }

                if (!costumer.Restriction)
                {
                    Console.WriteLine("Cliente não possui restrições.");
                    return null;
                }

                try
                {
                    connection.Execute(Customer.MoveToDeletedCustomer, new { Cpf = cpf });
                    return costumer;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro Deletar Cliente: " + ex.Message);
                    return null;
                }
            }
        }

        public async Task<bool> ChangeRestriction(string cpf)
        {
            using (var connection = new SqlConnection(_conn))
            {
                try
                {
                    var costumer = GetByCPF(cpf).Result;

                    if (costumer == null)
                        return false;

                    connection.Open();

                    string query = costumer.Restriction ? Customer.RemoveRestriction : Customer.AddRestriction;


                    var rowsAffected = connection.Execute(query, new { Cpf = cpf });
                    connection.Close();

                    if (rowsAffected > 0)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> EditCustomer(Customer updatedCustomer)
        {
            using (var connection = new SqlConnection(_conn))
            {
                try
                {
                    var costumer = GetByCPF(updatedCustomer.Cpf).Result;
                    if (costumer == null)
                        return false;

                    connection.Open();

                    var rowsAffected = connection.Execute(Customer.UpdateCustomer, new
                    {
                        Cpf = updatedCustomer.Cpf,
                        Name = updatedCustomer.Name,
                        BirthDate = updatedCustomer.BirthDate,
                        Gender = updatedCustomer.Gender,
                        Salary = updatedCustomer.Salary,
                        Phone = updatedCustomer.Phone,
                        Email = updatedCustomer.Email,
                        Address = updatedCustomer.Address,
                        Restriction = updatedCustomer.Restriction
                    });

                    connection.Close();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        private string FormatCpf(string cpf)
        {
            return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
        }

        private string FormatPhone(string phone)
        {
            return $"({phone.Substring(0, 2)}) {phone.Substring(2, 5)}-{phone.Substring(7, 4)}";
        }
    }
}
   

