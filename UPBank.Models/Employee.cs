using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UPBank.Models
{
    public class Employee : Person
    {
        public int Register;
        public bool Manager { get; set; }

        [NotMapped]
        public Agency Agency { get; set; }

        [JsonIgnore]
        public string AgencyNumber { get; set; } 

        /*
         * A annotation serve para que, ao criar a migration dessa entidade, o .Net não tente criar
         * uma tabela Agency no banco de dados, já que essa informação pertence à outra database, em uma outra API.
         * Através da API de Agency será possivel resgatar a Agency desse funcionário através desse atributo AgencyNumber (que será salvo no banco de dados de Employee) 
         * e popular o atributo Agency.
         * 
         * Se não forem usar Entity Framework, podem ignorar esse [NotMapped] e [JsonIgnore]
         */
    }
}
