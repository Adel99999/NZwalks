using System.ComponentModel.DataAnnotations;

namespace newzelandWalks.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a min of 3 char")]
        [MaxLength(3, ErrorMessage = "Code has to be a max of 3 char")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
