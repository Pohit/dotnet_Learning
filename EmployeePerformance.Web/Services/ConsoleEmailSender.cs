using Microsoft.AspNetCore.Identity.UI.Services;

namespace EmployeePerformance.Web.Services;

public class ConsoleEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine($"[EMAIL] To: {email}\nSubject: {subject}\n{htmlMessage}");
        return Task.CompletedTask;
    }
}

