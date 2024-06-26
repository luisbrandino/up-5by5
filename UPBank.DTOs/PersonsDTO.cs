using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UPBank.DTOs
{
    public class PersonsDTO
    {
        public string Cpf { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public char Gender { get; set; }
        public double Salary { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
