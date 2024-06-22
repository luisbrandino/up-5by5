using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UPBank.Models
{
    public class Agency
    {
        [Key]
        public string Number { get; set; }
        public string Cnpj { get; set; }
        public bool Restriction { get; set; }

        [NotMapped]
        public List<Employee> Employees { get; set; }

        /*
         * A annotation serve para que, ao criar a migration dessa entidade, o .Net não tente criar
         * esse campo de Employee no banco de dados, já que essa informação pertence à outra database, em uma outra API.
         * Através da API de Employee será possivel resgatar todos employees dessa agência e popular o atributo Employees
         */

        [NotMapped]
        public Address Address { get; set; }

        [JsonIgnore]
        public string AddressZipcode { get; set; }

        /*
         * Como vocês vão ter que pegar endereço de outro banco de dados, é preciso que 
         * tenha a chave do endereço aqui no Person para que vcs consigam pegar o endereço
         * na api e depois colocar o resposta da api com o endereço no atributo Address.
         * 
         * A annotation [JsonIgnore] é pra que quando vcs forem listar no controller ele 
         * não mostre esse campo, pois não é necessário
         */
    }
}
