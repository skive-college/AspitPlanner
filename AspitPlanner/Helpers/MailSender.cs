using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Helpers
{
    public class MailSender
    {
        public static void SendMail(string msg)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("jenslyn608@gmail.com");
                mail.To.Add("ke@skivecollege.dk");
                mail.To.Add("olfs@skivecollege.dk");
                mail.Subject = "AspitPlanner Error!";
                mail.Body = msg;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("Jenslyn608@gmail.com", "aspitmidtjylland");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                
            }
            catch (Exception ex)
            {
                FileHandler.Error(ex);
            }
        }

    }
}
