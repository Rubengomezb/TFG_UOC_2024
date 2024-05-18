using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.DTOs;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;

namespace TFG_UOC_2024.CORE.Services.Interfaces
{
    public interface IUserService
    {
        ServiceResponse<UserSearchDTO> SearchUsers(UserSearchInput req);
        Task<ServiceResponse<UserSimpleDTO>> GetUser(Guid id);

        Task<ServiceResponse<UserDTO>> GetSelf();
        Task<ServiceResponse<Guid>> AddUser(UserInput dto);
        Task<ServiceResponse<UserSimpleDTO>> UpdateUser(string id, UserSimpleDTO dto);
        Task<GenericResponse> DeleteUser(string id);
        Task<GenericResponse> UnDeleteUser(string id);

        Task<GenericResponse> UpdatePassword(string userid, PasswordChangeInput o);
        ServiceResponse<IEnumerable<UserRoleDTO>> GetUserRoles(Guid userid);
        Task<GenericResponse> AddRoleToUser(Guid userid, UserRoleInput req);
        Task<GenericResponse> DeleteRoleFromUser(Guid userid, Guid roleid);

        Task<IList<Claim>> GetRoleClaimsAsync(IList<string> roles);
    }
}
