using DSSPG4_WEB.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Services.MessageSevices
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public AuthMessageSender(IOptions<AuthMessageSMSSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSMSSenderOptions Options { get; }  // set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            //var sid = "AC006baa96869f80569a28ce11c16dee70";
            //var auth = "db35c54981036d930743927cfbdc3979";
            //var twilio = new TwilioRestClient(
            //   Options.SID,           // Account Sid from dashboard
            //   Options.AuthToken);    // Auth Token

            //var result = twilio.SendMessage(Options.SendNumber, number, message);
            // Use the debug output for testing without receiving a SMS message.
            // Remove the Debug.WriteLine(message) line after debugging.
            // System.Diagnostics.Debug.WriteLine(message);
            return Task.FromResult(0);
        }
    }

    public class AuthMessageSMSSenderOptions
    {
        public string SID { get; set; }
        public string AuthToken { get; set; }
        public string SendNumber { get; set; }
    }
}
