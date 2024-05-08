using BApp.Domain.DTOs.External;
using BApp.Domain.Enums;
using System;

namespace BApp.Domain.DTOs
{
    public class UserColorDTO
    {
        public int Id { get; set; }

        public DifficultyStatus Difficulty { get; set; }

        public DateTime SavingDate { get; set; }

        public ExternalColorDTO ExternalColor { get; set; }
    }
}
