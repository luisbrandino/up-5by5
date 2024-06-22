namespace UPBank.Addresses.Mongo.Settings
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string ConnectionString { get; set; } = "mongodb://root:Mongo%402024%23@localhost:27017/";
        public string DatabaseName { get; set; } = "UPBank";
        public string AddressCollectionName { get; set; } = "Address";
    }
}
