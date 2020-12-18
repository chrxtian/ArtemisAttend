using ArtemisAttend.API.DbContexts;
using ArtemisAttend.API.Entities;
using ArtemisAttend.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseLibrary.API.Services
{
    public class ArtemisAttendRepository : IArtemisAttendRepository, IDisposable
    {
        private readonly ArtemisAttendContext _context;

        public ArtemisAttendRepository(ArtemisAttendContext context )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // the repository fills the id (instead of using identity columns)
            user.Id = Guid.NewGuid();

            //foreach (var course in user.Courses)
            //{
            //    course.Id = Guid.NewGuid();
            //}

            _context.Users.Add(user);
        }

        public bool UserExists(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.Users.Any(a => a.Id == userId);
        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Remove(user);
        }
        
        public User GetUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.Users.FirstOrDefault(a => a.Id == userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList<User>();
        }

        public IEnumerable<User> GetUsers(UsersResourceParameters usersResourceParameters)
        {
            if (usersResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(usersResourceParameters));
            }

            if (string.IsNullOrWhiteSpace(usersResourceParameters.MainCategory) 
                && string.IsNullOrWhiteSpace(usersResourceParameters.SearchQuery))
            {
                return GetUsers();
            }

            var collection = _context.Users as IQueryable<User>;

            //if (!string.IsNullOrWhiteSpace(usersResourceParameters.MainCategory))
            //{
            //    usersResourceParameters.MainCategory = usersResourceParameters.MainCategory.Trim();
            //    collection = collection.Where(x => x.MainCategory == usersResourceParameters.MainCategory);
            //}
            if (!string.IsNullOrWhiteSpace(usersResourceParameters.SearchQuery))
            {
                usersResourceParameters.SearchQuery = usersResourceParameters.SearchQuery.Trim();
                collection = collection.Where(x => x.FirstName.Contains(usersResourceParameters.SearchQuery)
                || x.LastName.Contains(usersResourceParameters.SearchQuery));
            }

           
            
            return collection.ToList<User>();
        }

        public IEnumerable<User> GetUsers(IEnumerable<Guid> userIds)
        {
            if (userIds == null)
            {
                throw new ArgumentNullException(nameof(userIds));
            }

            return _context.Users.Where(a => userIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateUser(User user)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
               // dispose resources when needed
            }
        }
    }
}
