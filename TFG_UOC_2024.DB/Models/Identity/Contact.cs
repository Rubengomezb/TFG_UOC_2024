using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TFG_UOC_2024.DB.Models.BaseModels;

namespace TFG_UOC_2024.DB.Models.Identity
{
    public class Contact : BaseTrackedByModel
    {
        [MaxLength(300)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(300)]
        public string Email { get; set; }
        [MaxLength(500)]
        public string WebsiteUrl { get; set; }
        [MaxLength(30)]
        public string PhoneNumber { get; set; }

        public int FoodType { get; set; }

        public bool IsDeleted { get; set; }
    }
}
