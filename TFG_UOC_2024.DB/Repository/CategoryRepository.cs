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
    public class CategoryRepository : EntityRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<Category> GetIngredientByCategoryId(Guid categoryId)
        {
            return DbContext.Category.Include(c => c.Ingredients).Where(x => x.Id == categoryId);
        }

        public void UpsertRange(List<Category> categories) 
        {
            DbContext.Category.UpsertRange(categories);
        }
    }
}
