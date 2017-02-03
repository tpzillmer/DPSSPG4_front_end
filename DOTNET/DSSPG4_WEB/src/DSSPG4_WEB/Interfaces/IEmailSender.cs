using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
