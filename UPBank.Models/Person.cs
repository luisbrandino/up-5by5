using System.Text.Json.Serialization;

namespace UPBank.Models
{
    public abstract class Person
    {
        public string Cpf {  get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public char Gender { get; set; }
        public double Salary { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
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
