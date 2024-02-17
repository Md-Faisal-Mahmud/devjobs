using DevJobs.Application.DTOs;

namespace DevJobs.Application.Utilities
{
    public interface IAccountUtilities
    {
        string EncodePasswordResetToken(string code);
        string GenerateCallbackUrl(string origin, string userId, string code);
        public ForgotPasswordDTO ForgotPasswordReturn(bool isSuccess, string firstName, string lastName, string email, string callbackUrl);
        string DecodePasswordResetToken(string code);
        object GetTrueReturn();
        object GetFalseReturn();
        object GetLoginReturn(string token, string firstName, string lastName, string username, string email, string roleString, string imageData);
        string GetImageUrl(string userId);
        string GetRolesString(IEnumerable<string> roles);
        Task<bool> VerifyRecaptchaToken(string secretKey, string recaptchaToken);
    }
}
