using MovieStoreMvc.Repositories.Abstract;

namespace MovieStoreMvc.Repositories.Implementation
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _enviroment;

        public FileService(IWebHostEnvironment enviroment)
        {
            _enviroment = enviroment;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var wwwPath = _enviroment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".jfif"};
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(" ", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }

                //checking size
                long fileSizeInBytes = imageFile.Length;
                double fileSizeInMB = (double)fileSizeInBytes / (1024 * 1024); // Convert to megabytes
                if (fileSizeInMB > 10)
                {
                    return new Tuple<int, string>(0, "File size exceeds the maximum allowed (10 MB).");
                }
                string uniqueString = Guid.NewGuid().ToString();
                //create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = _enviroment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
