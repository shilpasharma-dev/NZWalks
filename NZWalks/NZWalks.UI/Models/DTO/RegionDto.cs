using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models.DTO
{
    public class RegionDto
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be 3 characters"),
           MaxLength(3, ErrorMessage = "Code has to be 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Region name is maximum 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
