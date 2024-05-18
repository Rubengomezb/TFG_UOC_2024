using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Models.BaseModels;

namespace TFG_UOC_2024.DB.Models
{
    public class Recipe : BaseTrackedByModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public Menu Menu {  get; set; }

        public string IngredientNames { get; set; }

        public double? Calories { get; set; }

        public double? Proteins { get; set; }

        public double? Carbohydrates { get; set; }

        public double? Fats { get; set; }
    }
}
