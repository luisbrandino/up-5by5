using Newtonsoft.Json;

namespace UPBank.Agencies.APIs.Utils
{
    public class ApiUtils<T>
    {
        public static async Task<T?> GetObjectFromResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(json))
                    return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }
    }
}