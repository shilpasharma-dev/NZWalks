using NZWalks.API.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name can not be longer than 100 characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Description can not be longer than 1000 characters")]
        public string Description { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Length in Km should be between 0 to 100 km")]
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}
