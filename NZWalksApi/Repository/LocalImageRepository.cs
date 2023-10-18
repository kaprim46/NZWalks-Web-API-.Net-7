using System;
using NZWalksApi.Data;
using NZWalksApi.Models.Domain;

namespace NZWalksApi.Repository
{
	public class LocalImageRepository : IImageRepository
	{
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext _db;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
                                    IHttpContextAccessor httpContextAccessor,
                                    NZWalksDbContext db)
		{
            _webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            //Upload Image to local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //https://localhost:1234/images/image.jpg
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            //Add Image to the images table
            await _db.Images.AddAsync(image);
            await _db.SaveChangesAsync();

            return image;
        }
    }
}

