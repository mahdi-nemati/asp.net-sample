namespace Endpoint.Infastructure
{
    public class LocalFileStorageAdapter : IFileAdapter
    {
        private readonly IHostEnvironment _environment;

        public LocalFileStorageAdapter(IHostEnvironment environment)
        {
            _environment = environment;
        }
        public string DeleteFile(string fileName, string path)
        {
            string webRootPath = _environment.ContentRootPath;
            string folderPath = Path.Combine(webRootPath, path);
            string finalPath = Path.Combine(webRootPath, fileName);

            if (File.Exists(finalPath))
            {
                File.Delete(finalPath);
                return finalPath;
            }
            return string.Empty;
        }

        public string InsertFile(IFormFile file, string path)
        {
            string webRootPath = _environment.ContentRootPath;
            string fileExtention = Path.GetExtension(file.FileName);
            string fileName = $"{Guid.NewGuid().ToString()}{fileExtention}";
            string folderForSave = Path.Combine(webRootPath, path);

            if (!Directory.Exists(folderForSave))
            {
                Directory.CreateDirectory(folderForSave);
            }

            using MemoryStream stream = new MemoryStream();
            file.CopyTo(stream);
            string finalFilePath = Path.Combine(folderForSave, fileName);
            File.WriteAllBytes(finalFilePath, stream.ToArray());

            return fileName;
        }

    }
}
