using DevSkill.Extensions.FileStorage.Services.Contracts;
using DevJobs.Application.Services;

namespace DevJobs.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileStoragePathService _fileStoragePathService;

        public FileService(IFileStorageService fileStorageService,IFileStoragePathService fileStoragePathService)
        {
            _fileStorageService = fileStorageService;
            _fileStoragePathService = fileStoragePathService;
        }

        public async Task<string> UploadFile(string base64File, string folderPath)
        {
            return await _fileStorageService.UploadAsync(base64File, folderPath);
        }

        public string GetFileUrl(string fileName, string defaultFile)
        {
            return _fileStoragePathService.GetFileUrl(fileName,defaultFile);
        }

        public async Task DeleteFile(string fileName)
        {
            await _fileStorageService.DeleteAsync(fileName);
        }
    }
}
