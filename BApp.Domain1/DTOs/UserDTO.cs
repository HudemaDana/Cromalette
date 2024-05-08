using System.Collections.Generic;

namespace BApp.Domain.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public LevelDTO Level { get; set; }

        public List<UserColorDTO> UserColors { get; set; }
    }
}
