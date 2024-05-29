using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IAccountRepository
    {
        int Login(Login user);
        (bool IsValidUser, int? NewUserId) UserLogin(Login user);
        ICollection<User> GetLoginUserDataBaseOnID(int Id);
        int RetrieveUserIdByUser(string UserName);
        bool AddedRegisterList(Login model);
        ICollection<LoginInfo> GetLoginIdByUsernameAndPasswordAsync(string userName, string password);
        bool UpdateLogin(loginwithallDetails model);
        bool UpdateCurrecyAdmin(UpdateCurrency model);
        string GetCurrecyAdmin();

        bool SetDefaultRouting(UpdateRouting model);
        ICollection<UpdateRouting> GetDefaultRouting();
    }
}
