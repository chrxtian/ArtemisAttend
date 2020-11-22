using ArtemisAttend.API.Entities;
using ArtemisAttend.API.ResourceParameters;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Services
{
    public interface IArtemisAttendRepository
    {    
        IEnumerable<Course> GetCourses(Guid userId);
        Course GetCourse(Guid userId, Guid courseId);
        void AddCourse(Guid userId, Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
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
