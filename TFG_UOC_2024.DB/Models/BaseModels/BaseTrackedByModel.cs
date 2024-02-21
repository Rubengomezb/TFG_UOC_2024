using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFG_UOC_2024.DB.Models.BaseModels
{
    public class BaseTrackedByModel : BaseTrackedModel
    {
        [Required]
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
