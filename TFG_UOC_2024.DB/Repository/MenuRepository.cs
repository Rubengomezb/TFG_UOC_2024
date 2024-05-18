using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.DB.Repository
{
    public class MenuRepository : EntityRepository<Menu>, IMenuRepository
    {
        public MenuRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<Menu> GetMenu(DateTime start, DateTime end, Guid userId)
        {
            return DbContext.Menu.Include(x => x.Recipe).Where(x => x.Date >= start && x.Date <= end &&  x.userId == userId);
        }

        public void UpsertRange(List<Menu> list)
        {
            DbContext.Menu.UpsertRange(list).Run();
        }
    }
}
