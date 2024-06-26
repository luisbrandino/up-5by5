namespace UPBank.Accounts.Api
{
    public interface IConsumer
    {
        public Task<T?> Get<T>(string url) where T : new();
    }
}
