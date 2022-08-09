using BLL.IServices;
using Microsoft.AspNetCore.SignalR;

namespace BLL.Services.ClientSide
{
    public class ClientHubService: Hub, IClientHubService
    {


        public async Task<string> GetUserToken(string? Key)
        {
            return Context.GetHttpContext().Request.Query[Key];
        }
    }
}
