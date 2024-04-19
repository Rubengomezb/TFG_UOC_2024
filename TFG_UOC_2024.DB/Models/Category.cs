using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Models.BaseModels;

namespace TFG_UOC_2024.DB.Models
{
    public  class Category : BaseTrackedByModel
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        [InverseProperty("CategoryNavigation")]
        public virtual ICollection<Ingredient> Ingredients { get;}
    }
}
