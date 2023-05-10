using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly static List<User> _user = new();
        public void Add(User user)
        {
            _user.Add(user);
        }

        public User? GetUserByEmail(string email)
        {
            return _user.SingleOrDefault(u => u.Email == email);
        }
    }
}