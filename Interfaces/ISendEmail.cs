namespace fiap_hacka.Interfaces
{
    public interface ISendEmail
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}
