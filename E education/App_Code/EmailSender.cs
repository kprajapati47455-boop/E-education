using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace E_education.App_Code
{
    public class EmailSender
    {
        internal string myemail {  get; set; }
        internal string mypass { get; set; }
        internal bool sendMyEmail(string SendTo,string Subject,string Msg)
        {
            myemail = "kprajapati47455@gmail.com";
            mypass = "spnz sloz llvd jhgo";
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                NetworkCredential nc = new NetworkCredential(myemail, mypass);
                smtp.Credentials = nc;
                smtp.EnableSsl = true;
                MailMessage message = new MailMessage();
                MailAddress mailfrom = new MailAddress(myemail);
                MailAddress mailto = new MailAddress(SendTo);
                message .From = mailfrom;
                message.Body = Msg;
                message.To.Add(SendTo);
                smtp.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}