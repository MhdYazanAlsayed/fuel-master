using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using Microsoft.AspNetCore.Http;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.FileStorage
{
    public interface IFileManager : IScopedDependency
    {
        Task<string> SaveAsync(string folderName, IFormFile file);
        void Delete(string folderName, string fileName);
    }
}
