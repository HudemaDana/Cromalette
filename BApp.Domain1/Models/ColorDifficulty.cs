using BApp.Domain.Enums;

namespace BApp.Domain.Models
{
    public class ColorDifficulty
    {
        public int Id { get; set; }
        
        public string ColorHexValue {  get; set; }
        
        public DifficultyStatus Status { get; set; }

        public int FindingCount {  get; set; }
    }
}
