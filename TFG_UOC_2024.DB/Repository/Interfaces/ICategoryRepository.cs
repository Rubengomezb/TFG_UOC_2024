using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.DB.Repository.Interfaces
{
    public  interface ICategoryRepository : IEntityRepository<Category>
    {
        IEnumerable<Category> GetIngredientByCategoryId(Guid categoryId);

        void UpsertRange(List<Category> categories);
    }
}
