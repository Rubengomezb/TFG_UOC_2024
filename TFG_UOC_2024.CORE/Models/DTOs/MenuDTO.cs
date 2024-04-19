using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.CORE.Models.DTOs
{
    public class MenuDTO
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public EatTime EatTime { get; set; }

        public Guid userId { get; set; }

        public RecipeDTO Recipe { get; set; }
    }
}
