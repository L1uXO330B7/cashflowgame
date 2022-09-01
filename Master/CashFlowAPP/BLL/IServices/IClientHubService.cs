namespace BLL.IServices
{
    public interface IClientHubService
    {
        Task GetUserToken(string? Key);
    }
}
