using System.Net;
using System.Net.Mail;
using Demo.DataAccess.Models;

namespace Demo.Presentation.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client = new SmtpClient("smtp.gmail.com",587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("abdelrahmanmohamed2293@gmail.com", "paxiydddqbhfgapw"); //need to 2 step verification
            Client.Send("abdelrahmanmohamed2293@gmail.com", email.To,email.Subject,email.Body);
        }
    }
}
