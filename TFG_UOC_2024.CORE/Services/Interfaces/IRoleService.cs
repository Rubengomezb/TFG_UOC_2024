using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.CORE.Services.Interfaces
{
    ServiceResponse<List<RoleDTO>> GetAll();
    Task<ServiceResponse<RoleDTO>> Get(Guid id);
    Task<ServiceResponse<Guid>> Add(RoleDTO role);
    Task<GenericResponse> Update(Guid id, RoleDTO role);
    Task<GenericResponse> DeleteRole(Guid roleid);
    ServiceResponse<List<ClaimDTO>> GetClaims();
    Task<GenericResponse> UpdateClaims(Guid roleid, List<string> claimValues);
}
