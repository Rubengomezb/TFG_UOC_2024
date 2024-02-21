using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.CORE.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse<UserDTO>> Login(Login model);
        Task<ServiceResponse<UserDTO>> ConfirmEmail(ConfirmInput model);
        Task<ServiceResponse<UserDTO>> ConfirmPassword(ConfirmInput model);
        Task<GenericResponse> ResetPassword(string email);
    }
}
