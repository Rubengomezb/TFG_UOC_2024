using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.CORE.Services.Base
{
    public abstract class HttpServiceBase : IDisposable
    {
        /// <summary>
        /// Http Client object
        /// </summary>
        protected readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServiceBase"/> class.
        /// </summary>
        public HttpServiceBase()
        {
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Creates a GET request
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <returns>Object deserialized</returns>
        [Obsolete]
        public async Task<dynamic> Get(string url)
        {
            var response = this.client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject(jsonString);
        }

        /// <summary>
        /// Creates a GET request by template
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <returns>Object deserialized</returns>
        public async Task<T> Get<T>(string url)
        {
            var response = this.client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// Creates a POST request
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <param name="args">Collection of parameters to send</param>
        /// <returns>Object deserialized</returns>
        public async Task<dynamic> Post(string url, Dictionary<string, object> args)
        {
            var content = new StringContent(JsonConvert.SerializeObject(args), Encoding.UTF8, "application/json");
            var response = this.client.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject(jsonString);
        }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose()
        {
            if (this.client != null)
            {
                this.client.Dispose();
            }
        }
    }
}
