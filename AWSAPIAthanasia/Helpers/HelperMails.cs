using System.Net.Mail;
using System.Net;

namespace ApiAthanasia.Helpers
{
    public class HelperMails
    {
        private IConfiguration configuration;

        public HelperMails(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private SmtpClient ConfigureSmtpClient()
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            string password = this.configuration.GetValue<string>("MailSettings:Credentials:Password");
            string hostname = this.configuration.GetValue<string>("MailSettings:ServerSmtp:Host");
            int port = this.configuration.GetValue<int>("MailSettings:ServerSmtp:Port");
            bool enableSSL = this.configuration.GetValue<bool>("MailSettings:ServerSmtp:EnableSsl");
            bool defaultCredentials = this.configuration.GetValue<bool>("MailSettings:ServerSmtp:DefaultCredentials");
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = hostname;
            smtpClient.Port = port;
            smtpClient.EnableSsl = enableSSL;
            smtpClient.UseDefaultCredentials = defaultCredentials;
            NetworkCredential credentials = new NetworkCredential(user, password);
            smtpClient.Credentials = credentials;
            return smtpClient;
        }

        private MailMessage ConfigureMailMessage(string para, string asunto, string mensaje)
        {
            MailMessage mail = new MailMessage();
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            mail.From = new MailAddress(user);
            mail.To.Add(para);
            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            return mail;
        }

        public async Task SendMailAsync(string para, string asunto, string mensaje)
        {
            MailMessage mail = this.ConfigureMailMessage(para, asunto, mensaje);
            SmtpClient smtpClient = this.ConfigureSmtpClient();
            await smtpClient.SendMailAsync(mail);
        }
    }
}
