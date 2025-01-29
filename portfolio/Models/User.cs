using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using portfolio.Data;

namespace portfolio.Models
{
    public class User
    {
            public int Id { get; set; }
            public string Username { get; set; }
            public int UserType { get; set; }
            public string PasswordHash { get; set; }  // Store hashed passwords

    }

    public interface IUserService
    {
        User? Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly MyDbContext _context;

        public UserService(MyDbContext context)
        {
            _context = context;
        }

        public User? Authenticate(string username, string password)
        {
            // Fetch the user from the database
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || password != user.PasswordHash)  // Make sure to securely hash passwords in real scenarios
                return null;

            return user;
        }

        //private readonly List<User> _users = new()
        //{
        //    new User { Id = 1, Username = "admin", PasswordHash = "123" }
        //};

        //public User? Authenticate(string username, string password)
        //{
        //    var user = _users.FirstOrDefault(u => u.Username == username);
        //    if (user == null || password != user.PasswordHash)
        //        return null;

        //    return user;
        //}
    }


}
