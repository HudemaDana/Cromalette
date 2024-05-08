using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BApp.Domain.Models
{
    public class UserLevel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int LevelId { get; set; } 

        public int CurrentXP { get; set; }

        public User User { get; set; }

        public Level Level { get; set; }
    }
}
