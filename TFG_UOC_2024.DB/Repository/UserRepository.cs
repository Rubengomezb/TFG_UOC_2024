using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.DB.Repository
{
    public class UserRepository : EntityRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return DbContext.Users
                     .Include(o => o.Contact)
                     .Include(o => o.UserRoles)
                         .ThenInclude(o => o.Role)
                     .AsQueryable();
        }
    }
}
