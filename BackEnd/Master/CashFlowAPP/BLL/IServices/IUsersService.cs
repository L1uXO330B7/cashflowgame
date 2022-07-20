using Common.Model;
using DPL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IUsersService<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs, ReadAllArgs> : ICrudService<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs>
    {
        Task<ApiResponse> ReadAll(ApiRequest<ReadAllArgs> Req);
    }
}
