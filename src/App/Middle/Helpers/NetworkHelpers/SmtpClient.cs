using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Configuration;
using System.Configuration;
using GridProteinFolding.Middle.Helpers.CryptographyHelpers;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using System.Threading;
using System.ComponentModel;

namespace GridProteinFolding.Middle.Helpers.NetworkHelpers
{
    public class SmtpClient
    {

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }

        }
        public bool Send(MailAddress to, string subject, string body)
        {
            SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            if (settings.Network.Password == null)
            {
                new Exception("Key (system.net/mailSettings/smtp) not is null!");
            }


            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //client.Credentials = new NetworkCredential(settings.Network.UserName, settings.Network.Password);
            client.Credentials = new NetworkCredential(settings.Network.UserName, settings.Network.Password);//CryptorEngine.Decrypt(settings.Network.Password, true));
            client.Port = settings.Network.Port;
            client.Host = settings.Network.Host;
            client.EnableSsl = true;

            try
            {
                MailAddress
                    maFrom = new MailAddress(settings.From, settings.From, Encoding.UTF8),
                    maTo = new MailAddress(to.Address, to.DisplayName, Encoding.UTF8);
                MailMessage mmsg = new MailMessage(maFrom.Address, maTo.Address);
                mmsg.Body = body;
                mmsg.BodyEncoding = Encoding.UTF8;
                mmsg.IsBodyHtml = true;
                mmsg.Subject = subject;
                mmsg.SubjectEncoding = Encoding.UTF8;

                client.SendCompleted += new
                    SendCompletedEventHandler(SendCompletedCallback);
                string userState = "SmtpClient.SendAsync";
                client.SendAsync(mmsg, userState);
                //client.Send(mmsg);
                Console.WriteLine("Sending message....");              

                return true;

            }
            catch (SmtpException ex)
            {
                GICO.WriteLine(ex);
                return false;
            }

        }
    }
}
