using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.DB.Repository.Interfaces
{
    public  interface IUserRoleRepository : IEntityRepository<ApplicationUserRole>
    {
        IEnumerable<ApplicationUserRole> GetUserRole(Guid id);
    }
}
