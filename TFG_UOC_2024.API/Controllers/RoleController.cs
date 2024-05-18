﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static TFG_UOC_2024.CORE.Components.Authorization.SystemClaims;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Services.Interfaces;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RoleController : BaseController
    {
        #region Constructor
        private RoleManager<ApplicationRole> _roleManager;
        private IRoleService roleService;

        public RoleController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IMapper mapper,
            ApplicationContext context,
            IRoleService rs,
            ILogger<RoleController> logger) : base(userManager, context, mapper, logger, configuration)
        {
            _roleManager = roleManager;
            roleService = rs;
        }
        #endregion

        #region Crud
        [Authorize(Policy = Permissions.Role.View)]
        [HttpGet]
        public ActionResult Get() =>
            Respond(roleService.GetAll());


        [Authorize(Policy = Permissions.Role.View)]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id) =>
            Respond(await roleService.Get(id));

        [Authorize(Policy = Permissions.Role.Create)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RoleDTO role) =>
            Respond(await roleService.Add(role));


        [Authorize(Policy = Permissions.Role.Edit)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] RoleDTO role) =>
            Respond(await roleService.Update(id, role));


        [Authorize(Policy = Permissions.Role.Delete)]
        [HttpDelete("{roleid}")]
        public async Task<ActionResult> DeleteRole(Guid roleid) =>
            Respond(await roleService.DeleteRole(roleid));

        #endregion

        #region Claims
        [Authorize(Policy = Permissions.Role.Edit)]
        [HttpGet("claims")]
        public IActionResult GetClaims() =>
            Respond(roleService.GetClaims());


        [Authorize(Policy = Permissions.Role.Edit)]
        [HttpPut("{roleid}/claims")]
        public async Task<IActionResult> UpdateClaims(Guid roleid, [FromBody] List<string> claimValues) =>
            Respond(await roleService.UpdateClaims(roleid, claimValues));

        #endregion
    }
}
