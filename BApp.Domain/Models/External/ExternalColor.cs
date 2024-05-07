using BApp.Domain.DTOs.External;

namespace BApp.Domain.Models.External
{
    public class ExternalColor
    {
        public HexDTO Hex { get; set; }
        public RGBDTO Rgb { get; set; }
        public HSLDTO Hsl { get; set; }
        public HSVDTO Hsv { get; set; }
        public NameDTO Name { get; set; }
        public CMYKDTO Cmyk { get; set; }
        public XYZDTO XYZ { get; set; }
    }
}
