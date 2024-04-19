using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Components;
using TFG_UOC_2024.CORE.Models.ApiModels;
using TFG_UOC_2024.CORE.Services.Base;

namespace TFG_UOC_2024.CORE.Clients
{
    public class HttpRecipeClient : HttpServiceBase, IHttpRecipeClient
    {
        private string baseApiUrl;

        private string token;

        private string baseApiId;

        private readonly IConfiguration _configuration;

        public HttpRecipeClient(IConfiguration configuration)
        {
            _configuration = configuration;
            this.ReadConfigParameters();
        }

        private void ReadConfigParameters()
        {
            var appSettingsSection = _configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            this.baseApiUrl = appSettings.BaseRecipeApiUrl;
            this.token = appSettings.RecipeApiToken;
            this.baseApiId = appSettings.RecipeApiId;
            //this.baseApiUrl = _configuration.GetValue<string>("BaseRecipeApiUrl");
            //this.token = _configuration.GetValue<string>("RecipeApiToken");
            //this.baseApiId = _configuration.GetValue<string>("RecipeApiId");
        }

        public async Task<RecipeResponse> GetRecipe(string filter)
        {
            var parameters = string.Format("?q={0}&app_id={1}&app_key={2}&from=0&to=50", filter, this.baseApiId, this.token);
            var requestUrl = string.Format("{0}{1}", this.baseApiUrl, parameters); ;

            return await this.Get<RecipeResponse>(requestUrl);
        }

        public async Task<RecipeResponse> GetRecipePaginated(string filter, int from, int to)
        {
            var parameters = string.Format("?q={0}&app_id={1}&app_key={2}&from={3}&to={4}", filter, this.baseApiId, this.token, from, to);
            var requestUrl = string.Format("{0}{1}", this.baseApiUrl, parameters); ;

            return await this.Get<RecipeResponse>(requestUrl);
        }
    }
}
