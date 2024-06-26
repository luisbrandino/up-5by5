using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UPBank.DTOs;

namespace UPBank.Models
{
    public abstract class Person
    {
        [Key]
        public string Cpf {  get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public char Gender { get; set; }
        public double Salary { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }

        [NotMapped]
        public Address Address { get; set; }

        [JsonIgnore]
        public string? AddressZipcode { get; set; }

        /* 
         * Como vocês vão ter que pegar endereço de outro banco de dados, é preciso que 
         * tenha a chave do endereço aqui no Person para que vcs consigam pegar o endereço
         * na api e depois colocar o resposta da api com o endereço no atributo Address.
         * 
         * A annotation [NotMapped] serve para que, ao rodar a migration, o .Net não tente criar a tabela Address.
         * 
         * A annotation [JsonIgnore] é pra que quando vcs forem listar no controller ele 
         * não mostre esse campo, pois não é necessário
         * 
         * Se não forem usar Entity Framework, podem ignorar as annotations [NotMapped] e [JsonIgnore]
         */

        public Person() { }

        public Person(PersonsDTO dto)
        {
            Cpf = dto.Cpf;
            Name = dto.Name;
            BirthDate = dto.BirthDate;
            Gender = dto.Gender;
            Salary = dto.Salary;
            Phone = dto.Phone;
            Email = dto.Email;
            Address = new Address { Zipcode = dto.Address };
        }
    }
}
