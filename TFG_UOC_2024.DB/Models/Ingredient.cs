using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Models.BaseModels;

namespace TFG_UOC_2024.DB.Models
{
    public class Ingredient : BaseTrackedByModel
    {
        public string Name {  get; set; }

        public string ImageUrl { get; set; }

        public Guid Recipe {  get; set; }

        public Guid Category { get; set; }

        [ForeignKey("Recipe")]
        [InverseProperty("Ingredients")]
        public virtual Recipe RecipeNavigation { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Ingredients")]
        public virtual Category CategoryNavigation { get; set; }
    }
}
