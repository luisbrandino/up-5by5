using UPBank.DTOs;

namespace UPBank.Models
{
    public class Customer : Person
    {
        public static readonly string InsertCustomer = "INSERT INTO Customer (Cpf, Name, BirthDate, Gender, Salary, Phone, Email, Address, Restriction) " +
            "VALUES (@Cpf, @Name, @BirthDate, @Gender, @Salary, @Phone, @Email, @Address, @Restriction)";

        public static readonly string SelectAllCustomers = @"
            SELECT Cpf, Name, BirthDate, Gender, Salary, Phone, Email, Address, Restriction
            FROM Customer";

        public static readonly string SelectAllDeletedCustomers = @"
            SELECT Cpf, Name, BirthDate, Gender, Salary, Phone, Email, Address, Restriction
            FROM DeletedCustomer";


        public static readonly string SelectCustomerByCpf = @"
            SELECT Cpf, Name, BirthDate, Gender, Salary, Phone, Email, Address, Restriction
            FROM Customer
            WHERE Cpf = @Cpf";

        public static readonly string UpdateCustomer = @"
            UPDATE Customer
            SET Name = @Name,
                Gender = @Gender,
                Salary = @Salary,
                Phone = @Phone,
                Email = @Email,
                Address = @Address
            WHERE Cpf = @Cpf";

        public static readonly string MoveToDeletedCustomer = @"
            BEGIN TRANSACTION;

            INSERT INTO DeletedCustomer (Cpf, [Name], BirthDate, Gender, Salary, Phone, Email, [Address], Restriction)
            SELECT Cpf, [Name], BirthDate, Gender, Salary, Phone, Email, [Address], Restriction
            FROM Customer
            WHERE Cpf = @Cpf;

            DELETE FROM Customer
            WHERE Cpf = @Cpf;

            COMMIT;";

        public static readonly string RemoveRestriction = "UPDATE Customer SET Restriction = 0 WHERE Cpf = @Cpf";
        public static readonly string AddRestriction = "UPDATE Customer SET Restriction = 1 WHERE Cpf = @Cpf";
        public bool Restriction { get; set; }

        public Customer() { }

        public Customer(CustomersDTO dTO) : base (dTO) {

            Restriction = true;
        }
    }
}
