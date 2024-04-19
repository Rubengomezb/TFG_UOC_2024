using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TFG_UOC_2024.CORE.Services.Base;
using TFG_UOC_2024.CORE.Services.Interfaces;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.CORE.Services.Menu
{
    public class MenuService : BaseService, IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        public MenuService(UserManager<ApplicationUser> u, IMapper m, IHttpContextAccessor hca, IMenuRepository menuRepository, ILogger logger = null, IConfiguration configuration = null) : base(u, m, hca, logger, configuration)
        {
            _menuRepository = menuRepository;
        }

        public IEnumerable<DB.Models.Menu> GetMenu(DateTime startTime, DateTime endTime, Guid userId)
        {
            return _menuRepository.GetMenu(startTime,endTime, userId).ToList();
        }

        public async Task<bool> CreateWeeklyMenu(List<DB.Models.Menu> menus)
        {
            _menuRepository.UpsertRange(menus);
            return true;
        }
    }
}
