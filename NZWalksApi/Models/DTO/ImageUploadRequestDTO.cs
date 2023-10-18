using System;
using System.ComponentModel.DataAnnotations;

namespace NZWalksApi.Models.DTO
{
	public class ImageUploadRequestDTO
	{
		[Required]
		public IFormFile File { get; set; }

		[Required]
		public string FileNme { get; set; }

		public string? FileDescription { get; set; }
	}
}

