using System.Collections.Generic;
using System.Linq;
using WebApiFingertec3._0.Models;

namespace WebApiFingertec3._0.Repositories
{
    public class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>();
            users.Add(new User { Id = 1, Username = "administrador", Password = "administrador" });
            return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
        }
    }
}
