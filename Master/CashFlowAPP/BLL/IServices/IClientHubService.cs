
using Microsoft.AspNetCore.SignalR;

namespace BLL.IServices
{
    public interface IClientHubService
    {
        Task<string> GetUserToken(string? Key);
    }
}
