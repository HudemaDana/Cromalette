namespace BApp.Domain.DTOs.External
{
    public class NameDTO
    {
        public string Value { get; set; }
        public string ClosestNamedHex { get; set; }
        public bool ExactMatchName { get; set; }
        public int Distance { get; set; }
    }
}
