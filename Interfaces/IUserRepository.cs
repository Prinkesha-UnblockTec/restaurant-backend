using restaurant.Models;

namespace restaurant.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetAllUserData();
        bool AddedUserList(User model);
        bool EditUser(User model);
        bool DeleteUser(int id);
    }
}
