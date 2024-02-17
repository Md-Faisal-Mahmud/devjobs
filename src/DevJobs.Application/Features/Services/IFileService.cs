namespace DevJobs.Application.Services
{
    public interface IFileService
    {
        Task<string> UploadFile(string base64File, string fileKey);
        string GetFileUrl(string fileName, string defaultFile);
        Task DeleteFile(string fileName);
    }
}
