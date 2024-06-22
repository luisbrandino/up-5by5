namespace UPBank.Addresses.Mongo.Settings
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string ConnectionString { get; set; } = "";
        public string DatabaseName { get; set; } = "UPBank";
        public string AddressCollectionName { get; set; } = "Address";
    }
}
