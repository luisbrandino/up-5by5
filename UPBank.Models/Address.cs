using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UPBank.Models
{
    public class Address
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [Key]
        public string Zipcode { get; set; }
        public int Number { get; set; }
        public string? Complement {  get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Neighborhood { get; set; }
    }
}
