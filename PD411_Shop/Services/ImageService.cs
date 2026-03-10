namespace PD411_Shop.Services
{
    public class ImageService
    {
        private IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment inv)
        {
            _env = inv;
        }

        public async Task<string?> SaveImageAsync(IFormFile file, string folder)
        {
            var types = file.ContentType.Split("/");
            if (types.Length != 2 || types[0] != "image") 
            {
                return null;
            } 

            string dirPath = Path.Combine(_env.WebRootPath, "images", folder);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string imagePath = Path.Combine(dirPath, imageName);
            using var stream = File.OpenWrite(imagePath);
            await file.CopyToAsync(stream);
            return imageName;
        }
        public void DeleteImage(string folder, string imageName)
        {
            string dirPath = Path.Combine(_env.WebRootPath, "images", folder);
            string imagePath = Path.Combine(dirPath, imageName);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

    }
}
