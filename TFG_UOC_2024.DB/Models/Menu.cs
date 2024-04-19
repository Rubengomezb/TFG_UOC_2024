using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Models.BaseModels;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.DB.Models
{
    public class Menu : BaseTrackedByModel
    {
        public DateTime Date { get; set; }

        public EatTime eatTime { get; set; }

        public Guid userId { get; set; }

        [InverseProperty("MenuNavigation")]
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
