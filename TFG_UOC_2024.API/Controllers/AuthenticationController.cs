﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;
using TFG_UOC_2024.CORE.Services.Interfaces;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.API.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController
    {
        #region Constructor
        private IUserService _userService;
        protected RoleManager<ApplicationRole> _roleManager;
        private IAuthenticationService _authService;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IMapper mapper,
            ApplicationContext dbContext,
            IUserService us,
            ILogger<AuthenticationController> logger,
            IAuthenticationService asvc) : base(userManager, dbContext, mapper, logger, configuration)
        {
            _roleManager = roleManager;
            _userService = us;
            _authService = asvc;
        }
        #endregion

        #region Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model) =>
            Respond(await _authService.Login(model));

        #endregion

        #region Validations
        [AllowAnonymous]
        [HttpPost("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmInput model) =>
            Respond(await _authService.ConfirmEmail(model));

        [AllowAnonymous]
        [HttpPost("confirmpassword")]
        public async Task<IActionResult> ConfirmPassword([FromBody] ConfirmInput model) =>
            Respond(await _authService.ConfirmPassword(model));

        [AllowAnonymous]
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email) =>
            Respond(await _authService.ResetPassword(email));

        #endregion
    }
}
