﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TFG_UOC_2024.CORE.Components.Authorization;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Services.Base;
using TFG_UOC_2024.CORE.Services.Interfaces;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.CORE.Services.User
{
    public class RoleService : BaseService, IRoleService
    {
        #region Constructor
        private RoleManager<ApplicationRole> _roleManager;
        private readonly IUserRoleRepository _userRoleRepository;

        public RoleService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IMapper mapper,
            IUserRoleRepository userRoleRepository,
            ILogger<RoleService> logger,
            IHttpContextAccessor hca) : base(userManager, mapper, hca, logger, configuration)
        {
            _roleManager = roleManager;
            _userRoleRepository = userRoleRepository;
        }
        #endregion

        #region Crud
        public ServiceResponse<List<RoleDTO>> GetAll()
        {
            var r = new ServiceResponse<List<RoleDTO>>();

            try
            {
                var roles = _roleManager.Roles.OrderBy(o => o.Name).ToList();

                // convert to DTO
                var dtos = mapper.Map<List<RoleDTO>>(roles);

                foreach (var d in dtos)
                {
                    d.Quantity = _userRoleRepository.Find(o => o.RoleId == d.Id).Count();
                }

                return r.Ok(dtos);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }


        public async Task<ServiceResponse<RoleDTO>> Get(Guid id)
        {
            var r = new ServiceResponse<RoleDTO>();

            var existing = await _roleManager.FindByIdAsync(id.ToString());

            if (existing == null)
                return r.NotFound("role not found");

            var dto = mapper.Map<RoleDTO>(existing);

            // add in the claims
            var claims = await _roleManager.GetClaimsAsync(existing);

            dto.Claims = mapper.Map<List<ClaimDTO>>(claims);

            return r.Ok(dto);
        }


        public async Task<ServiceResponse<Guid>> Add(RoleDTO role)
        {
            var r = new ServiceResponse<Guid>();

            // make sure it doesn't already exist
            if (await _roleManager.RoleExistsAsync(role.Name))
                return r.BadRequest("role already exists");

            // map to Role 
            var newRole = mapper.Map<ApplicationRole>(role);

            var res = await _roleManager.CreateAsync(newRole);

            if (!res.Succeeded)
                return r.BadRequest(res.Errors);

            return r.Ok(newRole.Id);
        }


        public async Task<GenericResponse> Update(Guid id, RoleDTO role)
        {
            var r = new GenericResponse();

            try
            {
                // make sure it exists
                var existing = await _roleManager.FindByIdAsync(role.Id.ToString());

                if (existing == null)
                    return r.NotFound("role not found");

                if (existing.NormalizedName == "ADMINISTRATOR")
                    return r.BadRequest("The Administrator cannot be updated.");

                // map updates
                existing.Name = role.Name;

                var res = await _roleManager.UpdateAsync(existing);

                // make sure it worked
                if (!res.Succeeded)
                    return r.BadRequest(res.Errors);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }

            return r.Ok();
        }


        public async Task<GenericResponse> DeleteRole(Guid roleid)
        {
            var r = new GenericResponse();

            // look up role
            var existing = await _roleManager.FindByIdAsync(roleid.ToString());

            if (existing == null)
                return r.NotFound("role not found");

            if (existing.NormalizedName == "ADMINISTRATOR")
                return r.BadRequest("The Administrator cannot be removed.");

            // see if they already have the role
            var result = await _roleManager.DeleteAsync(existing);

            if (result.Succeeded)
                return r.Ok();

            return r.BadRequest(result.Errors);
        }
        #endregion

        #region Claims

        public ServiceResponse<List<ClaimDTO>> GetClaims()
        {
            var r = new ServiceResponse<List<ClaimDTO>>();

            var dto = mapper.Map<List<ClaimDTO>>(SystemClaims.GetClaims());
            return r.Ok(dto);
        }


        public async Task<GenericResponse> UpdateClaims(Guid roleid, List<string> claimValues)
        {
            var r = new GenericResponse();

            // make sure the role exists
            var existing = await _roleManager.FindByIdAsync(roleid.ToString());

            if (existing == null)
                return r.NotFound("role not found");

            // get all claims in the system
            var allClaims = SystemClaims.GetClaims();

            // get any claims that this role has
            var roleClaims = await _roleManager.GetClaimsAsync(existing);

            // remove claims that don't exist in the new values
            foreach (var c in roleClaims)
            {
                if (!claimValues.Any(o => o == c.Value))
                    await _roleManager.RemoveClaimAsync(existing, c);
            }

            // add back newly selected claims
            foreach (var c in claimValues)
            {
                var claim = allClaims.FirstOrDefault(o => o.Value == c);
                if (claim == null)
                    continue;

                if (!roleClaims.Any(o => o.Value == claim.Value))
                    await _roleManager.AddClaimAsync(existing, claim);
            }

            return r.Ok();
        }
        #endregion
    }
}
