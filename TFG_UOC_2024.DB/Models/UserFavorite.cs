using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Models.BaseModels;

namespace TFG_UOC_2024.DB.Models
{
    public class UserFavorite : BaseTrackedByModel
    {
        public Guid UserId { get; set; }

        public Guid RecipeId { get; set; }
    }
}
