using System.ComponentModel.DataAnnotations.Schema;

namespace UPBank.Models
{
    public class Employee : Person
    {
        public int Register;
        public bool Manager { get; set; }

        [NotMapped]
        public Agency Agency { get; set; }

        /*
         * A annotation serve para que, ao criar a migration dessa entidade, o .Net não tente criar
         * uma tabela Agency no banco de dados, já que essa informação pertence à outra database, em uma outra API.
         * Através da API de Agency será possivel resgatar a Agency desse funcionário e popular o atributo Agency.
         * 
         * Se não forem usar Entity Framework, podem ignorar esse [NotMapped]
         */
    }
}
