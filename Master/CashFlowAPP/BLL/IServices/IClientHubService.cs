
using Microsoft.AspNetCore.SignalR;

namespace BLL.IServices
{
    public interface IClientHubService
    {
        Task GetUserToken(string? Key);
    }
}
