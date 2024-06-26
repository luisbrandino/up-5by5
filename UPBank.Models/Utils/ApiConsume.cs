using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UPBank.Models.Utils
{
    public class ApiConsume<T>
    {
        public static async Task<T?> Get(string uri, string requestUri)
        {
            T? generics;

            try
            {
                using HttpClient client = new();
                client.BaseAddress = new Uri(uri);

                HttpResponseMessage response = await client.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                generics = JsonConvert.DeserializeObject<T>(responseBody);
            }
            catch (Exception)
            {
                return default;
            }

            if (generics == null)
                return default;

            return generics;
        }

        public static async Task<T?> Post(string uri, string requestUri, dynamic postData)
        {
            T? generics;

            try
            {
                using HttpClient client = new();

                string jsonContent = JsonConvert.SerializeObject(postData);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri + requestUri, content);

                response.EnsureSuccessStatusCode();
                string strResponse = response.Content.ReadAsStringAsync().Result;

                generics = JsonConvert.DeserializeObject<T>(strResponse);
            }
            catch (Exception)
            {
                return default;
            }

            if (generics == null)
                return default;

            return generics;
        }

    }
}
