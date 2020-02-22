using GridProteinFolding.Middle.Helpers.NetworkHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblDistributed.Text = string.Format("{0}:{1}", "ServiceDistributed is ", (CheckHostDistributed() ? "ON" : "OFF"));

        lblDocument.Text = string.Format("{0}:{1}", "ServiceDocument is ", (CheckHostDocument() ? "ON" : "OFF"));

        lblEmail.Text = string.Format("{0}:{1}", "Send mail is ", (SendMail() ? "ON" : "ERROR"));

    }

    private bool CheckHostDocument()
    {
        ServiceDocument.DocumentManagmentClient objDocument = new ServiceDocument.DocumentManagmentClient();
        bool boolDocument = false;
        try
        {
            boolDocument = objDocument.DocumentEcho("Ping");
        }
        catch
        {
            boolDocument = false;
        }
        return boolDocument;
    }

    private bool CheckHostDistributed()
    {
        ServiceDistributed.ServiceClient objService = new ServiceDistributed.ServiceClient();
        bool boolService = false;
        try
        {
            string retService = objService.Echo();
            boolService = true;
        }
        catch
        {
            boolService = false;
        }
        return boolService;
    }

    private static bool SendMail()
    {
        bool boolEmail = false;
        //Sent e-mail
        SmtpClient smtpClient = new SmtpClient();

        try
        {
            string key = string.Empty;

            System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress("pablo.andrei@gmail.com", "pablo.andrei@gmail.com");
            string subject = "eFolding - TEST";
            string body = "Test of service...";

            boolEmail = smtpClient.Send(to, subject, body);

        }
        catch (Exception ex)
        {
            boolEmail = false;
        }
        finally
        {
            smtpClient = null;
        }

        return boolEmail;
    }
}