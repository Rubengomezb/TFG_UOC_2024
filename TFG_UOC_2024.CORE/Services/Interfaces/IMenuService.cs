using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.CORE.Services.Interfaces
{
    public interface IMenuService
    {
        IEnumerable<DB.Models.Menu> GetMenu(DateTime startTime, DateTime endTime, Guid userId);

        Task<bool> CreateWeeklyMenu(List<DB.Models.Menu> menus);
    }
}
