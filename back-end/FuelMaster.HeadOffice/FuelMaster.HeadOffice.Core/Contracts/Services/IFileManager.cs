using FuelMaster.HeadOffice.Core.Contracts.Markers;
using Microsoft.AspNetCore.Http;

namespace FuelMaster.HeadOffice.Core.Contracts.Services
{
    public interface IFileManager : IScopedDependency
    {
        Task<string> SaveAsync(string folderName, IFormFile file);
        void Delete(string folderName, string fileName);
    }
}
