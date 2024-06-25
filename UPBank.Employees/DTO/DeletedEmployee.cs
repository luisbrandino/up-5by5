using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UPBank.Employees.DTO;
using UPBank.Models;

namespace UPBank.Employees.DTO
{
    public class DeletedEmployee
    {
        [Key]
        public string Cpf { get; set; }
        public bool Manager { get; set; }
        public string  AgencyNumber { get; set; }
        public string  Name { get; set; }
        public DateTime BirthDate { get; set; }
        public char Gender { get; set; }
        public double Salary { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AddressZipcode { get; set; }
    }
}

