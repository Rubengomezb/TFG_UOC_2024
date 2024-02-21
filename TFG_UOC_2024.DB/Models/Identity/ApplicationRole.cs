using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TFG_UOC_2024.DB.Models.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}
