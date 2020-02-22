using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SandBox
{
    class TestSENDMAIL
    {
        ////class Program
        ////{



        ////    private static bool SendMail(GridProteinFoldingEntities ctx, Guid guid, string message, aspnet_Users user)
        ////    {
        ////        //Sent e-mail
        ////        GridProteinFolding.Middle.Helpers.NetworkHelpers.SmtpClient smtpClient = new GridProteinFolding.Middle.Helpers.NetworkHelpers.SmtpClient();
        ////        Process process = ctx.Process.FirstOrDefault(p => p.guid == guid);


        ////        if (!(process.emailNotification == Convert.ToByte(BasicEnums.EmailNotification.Enviar)))
        ////        {
        ////            try
        ////            {

        ////                System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(user.aspnet_Membership.LoweredEmail, user.UserName);
        ////                string subject = "eFolding - Simulation";
        ////                string body = "Simulation " + process.guid.ToString() + message;
        ////                body += "Direct access: efolding.fcfrp.usp.br/Pages/GCPS.aspx?guid= " + process.guid.ToString();

        ////                if (smtpClient.Send(to, subject, body))
        ////                    return true;
        ////                else
        ////                    return false;

        ////            }
        ////            catch (System.Net.Mail.SmtpException ex)
        ////            {
        ////                new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().SmtpException(ex, Types.ErrorLevel.Warning);
        ////                return false;
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                GICO.WriteLine(ex);
        ////                throw;
        ////            }
        ////            finally
        ////            {
        ////                smtpClient = null;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            return true;
        ////        }
        ////    }
        ////}
    }
}
