using System.ComponentModel.DataAnnotations.Schema;
using UPBank.Models;

namespace UPBank.Employees.DTO
{
    public class EmployeeDTO
    {
        public string Cpf {  get; set; }
        public string Name {  get; set; }
        public char Gender {  get; set; }
        public double Salary {  get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate {  get; set; }
        public string AddressZipcode { get; set; }
        public int Register;
        public bool Manager { get; set; }
        public string AgencyNumber { get; set; }
    }
}
