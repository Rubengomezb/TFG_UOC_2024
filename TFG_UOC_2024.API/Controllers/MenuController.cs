using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TFG_UOC_2024.CORE.Managers.Interfaces;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MenuController : BaseController
    {
        private readonly IMenuManager _menuManager;
        public MenuController(UserManager<ApplicationUser> u, ApplicationContext dbContext, IMapper m, IMenuManager menuManager, ILogger logger = null, IConfiguration configuration = null) : base(u, dbContext, m, logger, configuration)
        {
            _menuManager = menuManager;
        }

        [HttpGet("menu")]
        public async Task<ActionResult> GetWeeklyMenu([FromQuery] DateTime startDate, [FromQuery] DateTime endDate) =>
            Respond(await _menuManager.GetMenu(startDate, endDate));

        [HttpPost("menu")]
        public async Task<ActionResult> CreateMenu([FromBody] DateTime startDate, [FromBody] DateTime endDate) =>
            Respond(await _menuManager.CreateMenu( startDate, endDate));
    }
}
