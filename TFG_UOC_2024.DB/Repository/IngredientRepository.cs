using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.DB.Repository
{
    public class IngredientRepository : EntityRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(ApplicationContext context) : base(context)
        {
        }

        public void UpsertRange(List<Ingredient> ingredients)
        {
            DbContext.Ingredient.UpsertRange(ingredients).Run();
        }
    }
}
