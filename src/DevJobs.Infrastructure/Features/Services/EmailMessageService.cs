using System.Text.Encodings.Web;
using DevJobs.Application.Features.Services;
using DevJobs.Application.Utilities;
using DevJobs.Infrastructure.Templates;
using DevSkill.Http.Emails.Services;

namespace DevJobs.Infrastructure.Features.Services
{
    public class EmailMessageService(IQueuedEmailService queuedEmailService, IAccountUtilities accountUtilities) : IEmailMessageService
    {
        public async Task<object> SendResetPasswordEmailAsync(bool isUserExist, string receiverName,
            string receiverEmail, string resetPasswordLink)
        {
            if (isUserExist)
            {
                var template = new ResetPasswordTemplate(
                    receiverName, HtmlEncoder.Default.Encode(resetPasswordLink));
                string body = template.TransformText();
                string subject = "Reset Your Password";
                await queuedEmailService.SendSingleEmailAsync(receiverName, receiverEmail, subject, body);

                return accountUtilities.GetTrueReturn();
            }
            else
            {
                return accountUtilities.GetFalseReturn();
            }
        }
    }
}
