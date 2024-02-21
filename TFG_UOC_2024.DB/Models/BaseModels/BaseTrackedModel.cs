using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TFG_UOC_2024.DB.Models.BaseModels
{
    public class BaseTrackedModel : BaseModel
    {
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
