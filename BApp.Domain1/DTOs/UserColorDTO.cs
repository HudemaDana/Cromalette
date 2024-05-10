using BApp.Domain.Enums;
using System;

namespace BApp.Domain.DTOs
{
    public class UserColorDTO
    {
        public int Id { get; set; }

        public DifficultyStatus Difficulty { get; set; }

        public DateTime SavingDate { get; set; }

        public string ColorHexValue { get; set; }
    }
}
