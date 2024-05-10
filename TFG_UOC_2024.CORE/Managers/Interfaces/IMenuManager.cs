using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Models;

namespace TFG_UOC_2024.CORE.Managers.Interfaces
{
    public interface IMenuManager
    {
        Task<ServiceResponse<IEnumerable<MenuDTO>>> GetMenu(DateTime startTime, DateTime endTime);

        Task<ServiceResponse<bool>> CreateMenu(DateTime startTime, DateTime endTime);
    }
}
