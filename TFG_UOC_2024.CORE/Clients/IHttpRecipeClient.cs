using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models.ApiModels;

namespace TFG_UOC_2024.CORE.Clients
{
    public interface IHttpRecipeClient
    {
        Task<RecipeResponse> GetRecipe(string filter);

        Task<RecipeResponse> GetRecipePaginated(string filter, int from, int to);
    }
}
