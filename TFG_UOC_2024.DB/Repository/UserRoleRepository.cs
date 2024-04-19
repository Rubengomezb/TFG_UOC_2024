using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.DB.Repository
{
    public class UserRoleRepository : EntityRepository<ApplicationUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<ApplicationUserRole> GetUserRole(Guid id)
        {
            return DbContext.UserRoles.Include(o => o.Role)
                                    .Where(o => o.UserId == id);
        }
    }
}
