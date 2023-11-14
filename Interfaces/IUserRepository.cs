using Hikers_Diary.DTO;
using HikersDiary_Web.Model;

namespace Hikers_Diary.Interfaces
{
    public interface IUserRepository
    {
        /*ICollection<User> GetUsers();*/
     
        UserDto GetUserById(int userId);
        UserDto GetUserByUsername(string username);
        IEnumerable<UserDto> SearchUsers(string searchQuery);
        void AddUser(UserDto user);
        void UpdateUser(UserDto user);
        public UserDto ValidateUser(string username, string password);



    }
}
