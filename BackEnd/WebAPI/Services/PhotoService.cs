using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IConfiguration config)
        {
            
            Account account = new Account(
                config.GetSection("CloudinarySettings:CloudName").Value,
                // Read from environment variables
                config.GetSection("CloudinarySettings:ApiKey").Value, // Using NESTFINDER_ prefix
                config.GetSection("CloudinarySettings:ApiSecret").Value // Using NESTFINDER_ prefix
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo)
        {
            var uploadResult = new ImageUploadResult();
            if (photo.Length > 0)
            {
                using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation()
                        .Height(500).Width(800)
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }
    }
}
