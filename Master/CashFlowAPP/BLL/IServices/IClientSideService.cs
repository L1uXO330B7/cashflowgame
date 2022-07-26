using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IClientSideService
    {
        Task<ApiResponse> UserSignUp(ApiRequest<UserSignUpDTO> Req);

    }
}
