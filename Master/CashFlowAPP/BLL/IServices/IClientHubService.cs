
using Microsoft.AspNetCore.SignalR;

namespace BLL.IServices
{
    public interface IClientHubService
    {
        string? GetUserToken(string? Key);
    }
}
