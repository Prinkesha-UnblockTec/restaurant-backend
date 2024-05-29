using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetAllUserData();
        bool AddedUserList(User model);
      bool EditChangePassword(changePasswordforusercrud model);
        bool EditUser(User model);
        bool DeleteUser(int id);
    }
}
