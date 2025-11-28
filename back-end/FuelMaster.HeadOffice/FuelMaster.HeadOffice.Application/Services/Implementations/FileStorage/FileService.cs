using FuelMaster.HeadOffice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.FileStorage
{
    public class FileManager : IFileManager
    {
        private readonly IHostEnvironment _environment;

        public FileManager(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public void Delete(string folderName, string fileName)
        {
            string path = Path.Combine(_environment.ContentRootPath, "wwwroot", folderName, fileName);

            if (File.Exists(path)) File.Delete(path);
        }

        public async Task<string> SaveAsync(string folderName, IFormFile file)
        {
            string extension = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string fileName = $"{Guid.NewGuid() + extension}";
            string path = Path.Combine(_environment.ContentRootPath, "wwwroot", folderName, fileName);

            await file.CopyToAsync(new FileStream(path, FileMode.Create));

            return fileName;
        }
    }
}
