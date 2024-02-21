using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TFG_UOC_2024.DB.Models.Identity
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
