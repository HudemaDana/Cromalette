using BApp.Domain.Enums;
using BApp.Domain.Models;
using System;

namespace BApp.Domain.DTOs
{
    public class UserColorDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ColorHexValue { get; set; }

        public DifficultyStatus Difficulty { get; set; }

        public DateTime SavingDate { get; set; }

        public User User { get; set; }
    }
}
