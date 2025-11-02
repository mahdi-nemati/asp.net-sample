namespace Endpoint.Infastructure
{
    public interface IFileAdapter
    {
        public string InsertFile(IFormFile file, string path);
        public string DeleteFile(string fileName, string path);

        public string UpdateFile(string oldFileName, IFormFile file, string path)
        {
            DeleteFile(oldFileName, path);
            return InsertFile(file, path);
        }
    }
}
