﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TFG_UOC_2024.CORE.Components;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.CORE.Services.Base
{
    public class BaseService
    {
        public const string AdministratorUser = "ADMINISTRATOR";

        // define db context
        protected UserManager<ApplicationUser> userManager;
        protected IMapper mapper { get; set; }
        protected Microsoft.Extensions.Logging.ILogger log;
        protected IConfiguration config;
        protected IHttpContextAccessor httpContextAccessor;

        public BaseService(
            UserManager<ApplicationUser> u,
            IMapper m,
            IHttpContextAccessor hca,
            Microsoft.Extensions.Logging.ILogger logger = null,
            IConfiguration configuration = null)
        {
            this.userManager = u;
            this.mapper = m;
            this.log = logger;
            this.config = configuration;
            httpContextAccessor = hca;
        }

        protected async Task<ApplicationUser> GetUser()
        {
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return null;

            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var u = await userManager.FindByIdAsync(userId);

            if (u == null)
                return null;

            return u;
        }

        protected Guid GetUserId()
        {
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return Guid.Empty;

            return httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToGuid();
        }

        protected async Task<string> GetUsername()
        {
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return string.Empty;


            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return string.Empty;

            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var u = await userManager.FindByIdAsync(userId);

            if (u == null)
                return string.Empty;

            return u.UserName;
        }
    }
}
