using System.Collections.Generic;

namespace BApp.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Level Level { get; set; }

        public List<UserColor> UserColors { get; set; }
    }
}
