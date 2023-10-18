using System;
using System.ComponentModel.DataAnnotations;

namespace NZWalksApi.Models.DTO
{
	public class RegionCreateDTO
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be a minimum of 3 chracters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 chracters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Code has to be a maximum of 100 chracters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

