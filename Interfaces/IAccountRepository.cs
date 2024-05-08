using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IAccountRepository
    {
        int Login(Login user);
        ICollection<LoginInfo> GetLoginIdByUsernameAndPasswordAsync(string userName, string password);
        bool UpdateLogin(loginwithallDetails model);
    }
}
