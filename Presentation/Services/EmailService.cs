using Azure.Communication.Email;
using Azure;
using Microsoft.Extensions.Configuration;
using Presentation.Models;

namespace Presentation.Services;

public class EmailService(IConfiguration config) : IEmailService
{
    #region ChatGPT Advice ()

        private readonly IConfiguration _config = config;

        public async Task<ServiceResult<string>> SendVerificationEmailAsync(string toEmail)
        {
            string connectionString = _config["Azure:CommunicationServices:ConnectionString"]!;
            var emailClient = new EmailClient(connectionString);

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@a7039b22-7128-4a98-92bf-695e77c169cf.azurecomm.net",
                content: new EmailContent("Verification Email")
                {
                    PlainText = "Please verify your email.",
                    Html = @"<html><body><h1>Please verify your email</h1></body></html>"
                },
                recipients: new EmailRecipients(new List<EmailAddress> { new(toEmail) }));

            try
            {
                EmailSendOperation operation = await emailClient.SendAsync(WaitUntil.Completed, emailMessage);

                return new ServiceResult<string>
                {
                    Succeeded = true,
                    StatusCode = 200,
                    Result = operation.Id
                };
            }
            catch (RequestFailedException ex)
            {
                return new ServiceResult<string>
                {
                    Succeeded = false,
                    StatusCode = (int)ex.Status,
                    Error = ex.Message
                };
            }
        }

    #endregion
}