using BApp.Domain.Models;


namespace BApp.Domain.DTOs
{
    public class UserLevelDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int LevelId { get; set; }

        public int CurrentXP { get; set; }

        public UserDTO User { get; set; }

        public LevelDTO Level { get; set; }
    }
}
