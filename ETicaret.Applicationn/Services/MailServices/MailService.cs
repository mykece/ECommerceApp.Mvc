using ETicaret.Applicationn.DTOs.MailDTOs;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.MailServices
{
    public class MailService : IMailService
    {
        public async Task SendMailAsync(SendMailDTO sendMailDTO)
        {
			try
			{
				var newMail = new MimeMessage();
				newMail.From.Add(MailboxAddress.Parse("bilgeadamlar09@gmail.com"));
				newMail.To.Add(MailboxAddress.Parse(sendMailDTO.Email));
				newMail.Subject = sendMailDTO.Subject;
				var builder = new BodyBuilder();
				builder.HtmlBody = sendMailDTO.Message;
				newMail.Body = builder.ToMessageBody();
				var smtp = new SmtpClient();
				await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
				await smtp.AuthenticateAsync("bilgeadamlar09@gmail.com", "snslyoexluzhurzh");
				await smtp.SendAsync(newMail);
				await smtp.DisconnectAsync(true);
			}
			catch (Exception ex)
			{

				throw new InvalidOperationException($"There was an error sending the Email : {ex.Message}");
			}
        }
    }
}
