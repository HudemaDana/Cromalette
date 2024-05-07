using BApp.Domain.Enums;
using BApp.Domain.Models.External;
using System;

namespace BApp.Domain.Models
{
    public class UserColor
    {
        public int Id { get; set; }

        public DifficultyStatus Difficulty { get; set; }

        public DateTime SavingDate { get; set; }

        public ExternalColor ExternalColor { get; set; }
    }
}
