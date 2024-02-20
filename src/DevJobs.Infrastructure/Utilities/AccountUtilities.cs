using DevJobs.Application.DTOs;
using DevJobs.Application.Services;
using DevJobs.Application.Utilities;
using DevJobs.Domain.Utilities;
using DevSkill.Extensions.FileStorage.Options;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace DevJobs.Infrastructure.Utilities
{
    public class AccountUtilities : IAccountUtilities
    {
        private IFileService _fileService;
        private FileStorageSetting _storageSettings;
        public AccountUtilities(IFileService fileService, IOptions<FileStorageSetting> storageSettings) 
        { 
            _fileService = fileService;
            _storageSettings = storageSettings.Value;
        }

        public string EncodePasswordResetToken(string code)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        }

        public string GenerateCallbackUrl(string origin, string userId, string code)
        {
            return $"{origin}/reset-password?userId={userId}&code={code}";
        }

        public ForgotPasswordDTO ForgotPasswordReturn(bool isSuccess, string firstName, string lastName, string email, string callbackUrl)
        {
            var forgotPasswordDTO = new ForgotPasswordDTO
            {
                IsSuccess = isSuccess,
                FullName = $"{firstName} {lastName}",
                Email = email,
                CallbackUrl = callbackUrl
            };

            return forgotPasswordDTO;
        }

        public string DecodePasswordResetToken(string code)
        {
            var decodedTokenByte = WebEncoders.Base64UrlDecode(code);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenByte);
            return decodedToken;
        }

        public object GetTrueReturn()
        {
            return new { success = true };
        }

        public object GetFalseReturn()
        {
            return new { success = false };
        }

        public object GetLoginReturn(string token, string firstName, string lastName, string username, string email, string role, string image)
        {
            var name = $"{firstName} {lastName}";
            return new { success = true, token, name, username, email, role, image };
        }

        public string GetImageUrl(string imageName)
        {
            if(!string.IsNullOrWhiteSpace(imageName))
            return _fileService.GetFileUrl($"{_storageSettings.FolderPaths[AWSFolder.ProfileImages]}/{imageName}", $"{_storageSettings.FolderPaths[AWSFolder.ProfileImages]}/{imageName}");
            return "";
        }

        public string GetRolesString(IEnumerable<string> roles)
        {
            return string.Join(", ", roles);
        }

        public async Task<bool> VerifyRecaptchaToken(string secretKey, string recaptchaToken)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify",
                    new StringContent($"secret={secretKey}&response={recaptchaToken}",
                    Encoding.UTF8, "application/x-www-form-urlencoded"));

                response.EnsureSuccessStatusCode();
                var verificationResponse = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(verificationResponse);
                var success = jsonDocument.RootElement.GetProperty("success").GetBoolean();

                return success;
            }
        }
    }
}
