namespace UPBank.Addresses.Mongo.Settings
{
    public interface IMongoDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string AddressCollectionName { get; set; }
    }
}
