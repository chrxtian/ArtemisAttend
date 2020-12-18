using ArtemisAttend.API.Entities;
using ArtemisAttend.API.ResourceParameters;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Services
{
    public interface IArtemisAttendRepository
    {   
        IEnumerable<User> GetUsers();
        IEnumerable<User> GetUsers(UsersResourceParameters usersResourceParameters);
        User GetUser(Guid userId);
        IEnumerable<User> GetUsers(IEnumerable<Guid> userIds);
        void AddUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
        bool UserExists(Guid userId);
        bool Save();
    }
}
