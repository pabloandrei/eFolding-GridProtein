using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using GridProteinFolding.Entities.Internal;
using GridProteinFolding.Entities.Membership;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.NetworkHelpers;
using System.Text.RegularExpressions;

public partial class Pages_SimulationAdd : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request["guid"] != null)
            {
                Guid guid = new Guid(Request["guid"]);
                ViewState.Add("guid", guid);
                GetData();

                lblLegend.Text = "Edit experiment";
            }
            else
            {
                ViewState.Add("guid", Guid.Empty);
                lblLegend.Text = "Add experiment";
            }

        }

        Interation();

    }

    protected bool CustomValidate()
    {
        var valid = true;
        string errorMessage = string.Empty;
        var test = new Regex("");


        #region TEMPERATURA
        test = new Regex(@"([0-9,\.])+(;([0-9,\.])+)*");
        bool isValid = test.IsMatch(txtTemperature.Text.Trim());
        if (!isValid)
            errorMessage += "Inclusão de uma Temperatura é requerida!";
        #endregion

        #region MODELTOHP
        if (ddlModel.SelectedValue.Equals("3"))
        {
            test = new Regex(@"[^HP;]");
            isValid = !(test.IsMatch(txtModelHP.Text.Trim()));

            string[] modelHP = txtModelHP.Text.Split(';').ToArray<string>();
            isValid = !(modelHP.Count() == Convert.ToInt32(txtMaxInterations.Text));

            if (!isValid)
                errorMessage += "Inclusão do Model HP é requerida!";
        }
        #endregion

        var validator = new CustomValidator();
        validator.IsValid = isValid;
        valid = validator.IsValid;
        validator.ErrorMessage = errorMessage;
        this.Page.Validators.Add(validator);

        return valid;
    }

    protected void bttProcess_Click(object sender, EventArgs e)
    {

        if (IsPostBack)
        {
            Page.Validate();
            var valid = CustomValidate();

            if (valid && Page.IsValid)
            {
                Guid guid = new Guid(ViewState["guid"].ToString());

                if (guid == Guid.Empty)
                {
                    Create();
                }
                else
                {
                    Edit();
                }
            }
        }

    }

    private void Create()
    {
        //lblLog.Visible = false;
        //byte[] appData = null;
        //bool uploadFile = false;

        //foreach (string fileName in Request.Files)
        //{

        //    HttpPostedFileBase postedFile = Request.Files[fileName];

        //    if (postedFile.FileName != string.Empty)
        //    {
        //        uploadFile = true;
        //        Stream s = postedFile.InputStream;
        //        appData = new byte[postedFile.ContentLength + 1];
        //        s.Read(appData, 0, postedFile.ContentLength);
        //    }
        //}

        //load last CONFIGAPP
        ConfigApp lastConfigApp;
        using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        {
            lastConfigApp = ctx.ConfigApp.FirstOrDefault();
        }

        using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        {
            bool first = true;
            Guid guidFather = new Guid();

            double[] tempers = txtTemperature.Text.Split(';').Select(double.Parse).ToArray();

            for (int t = 0; t < tempers.Length; t++)
            {
                double temper = tempers[t];
                #region NEW PROCESS
                Process process = new Process();
                process.guid = Guid.NewGuid();
                process.date = DateTime.Now;
                process.userId = GetUserID();
                process.status_id = 0; //=CRIADO
                process.note = txtNote.Text.ToString().Trim();
                process.emailNotification = Convert.ToByte(chkEmail.Checked);
                process.label = txtLabel.Text.ToString().Trim();

                process.configApp_id = lastConfigApp.id;

                if (!first)
                {
                    process.guidFather = guidFather;
                }
                ctx.Process.Add(process);
                #endregion

                #region NEW DATATORESULT
                DataToResult dataToResult = new DataToResult();
                dataToResult.process_guid = process.guid;
                dataToResult.valueDiscard = Convert.ToInt32(txtValueDiscard.Text);
                dataToResult.valueDivResult = Convert.ToInt32(txtValueDivResult.Text);
                ctx.DataToResult.Add(dataToResult);
                #endregion


                #region DATATOPROCESS
                DataToProcess dataToProcess = new DataToProcess();
                //dataToProcess.loadDatFile = uploadFile;

                //if (uploadFile)
                //{

                //    dataToProcess.file = new System.Text.ASCIIEncoding().GetString(appData);
                //    dataToProcess.isem = Convert.ToInt32(txtIsem.Text.Trim());
                //    dataToProcess.maxInterations = 1;
                //    dataToProcess.numberOfDelta = Convert.ToDouble(txtNumberOfDelta.Text.Trim());
                //    dataToProcess.calcDelta = true;
                //    dataToProcess.process_guid = process.guid;
                //    //string[] lines = Regex.Split(dataToProcess.file, "\r\n");
                //    dataToProcess.totalSitio = lines.Length - 1;
                //}

                //else
                //{
                dataToProcess.file = null;
                dataToProcess.isem = Convert.ToInt32(txtIsem.Text.Trim());
                dataToProcess.maxInterations = Convert.ToInt32(txtMaxInterations.Text.Trim());
                //P dataToProcess.valueOfDelta = Convert.ToDouble(txtValueOfDelta.Text.Trim());
                dataToProcess.process_guid = process.guid;
                dataToProcess.totalSitio = Convert.ToInt32(txtTotalSitio.Text.Trim());
                dataToProcess.modelType = Convert.ToByte(ddlModel.SelectedValue);
                dataToProcess.beta = 1; // Convert.ToDouble(txtBeta.Text.Trim());
                dataToProcess.maxMotionPeerIsem = Convert.ToInt64(txtMaxMotionPeerIsem.Text.Trim());
                dataToProcess.temperature = temper;// Convert.ToDouble(txtTemperature.Text);
                dataToProcess.loadDatFile = null;
                dataToProcess.recPathEvery = Convert.ToInt32(txtRecPathEvery.Text.Trim());
                dataToProcess.splitFileEvery = Convert.ToInt32(txtSplitFileEvery.Text.Trim());
                //}
                ctx.DataToProcess.Add(dataToProcess);
                #endregion

                Output output = new Output();
                output.guid = process.guid;
                output.evolution = chkEvolution.Checked;
                output.distribution = chkDistribution.Checked;
                output.configuration = chkConfiguration.Checked;
                //output.configurationJumpStep = Convert.ToInt32(txtConfigRange.Text.Trim());                
                output.debug = chkDebug.Checked;
                output.trajectory = true;

                ctx.Output.Add(output);

                if (dataToProcess.modelType == 3)
                { //ModelTYPE HP
                    string[] modelHP = txtModelHP.Text.Split(';').ToArray<string>();
                    for (int i = 0; i < modelHP.Count(); i++)
                    {
                        Model model = new Model();
                        model.process_guid = process.guid;
                        model.monomero = (byte)i;
                        model.value = Convert.ToDouble(modelHP[i] == "H" ? 0 : 1);

                        ctx.Model.Add(model);
                    }
                }
                else
                {
                    //Demais ModelTYPE
                    #region MODEL
                    for (int i = 0; i < dataToProcess.totalSitio; i++)
                    {
                        Model model = new Model();
                        model.process_guid = process.guid;
                        model.monomero = (byte)i;

                        if (dataToProcess.modelType == 0)
                        {
                            //ModelTYPE Random
                            model.value = 0;
                        }
                        else if (dataToProcess.modelType == 1)
                        {
                            //ModelTYPE Negative
                            model.value = -1;
                        }
                        else if (dataToProcess.modelType == 2)
                        {
                            //ModelTYPE Negative
                            model.value = 1;
                        }

                        ctx.Model.Add(model);
                    }
                    #endregion                    
                }

                ctx.SaveChanges();

                if (first)
                {
                    guidFather = process.guid;
                    first = false;
                }

                if (Convert.ToBoolean(process.emailNotification))
                    SendMail(ctx, process.guid, " was created!", Application["webServerName"].ToString());
            }



            Response.Redirect("~/Pages/Simulation.aspx");

        }

    }

    private static void SendMail(GridProteinFoldingEntities ctx, Guid guid, string message, string webServerName)
    {

        //Sent e-mail
        SmtpClient smtpClient = new SmtpClient();
        Process process = ctx.Process.FirstOrDefault(p => p.guid == guid);
        aspnet_Users user = new GridProteinFolding_MemberShipEntities().aspnet_Users.FirstOrDefault(u => u.UserId == process.userId);

        try
        {
            string key = string.Empty;

            System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(user.aspnet_Membership.LoweredEmail, user.UserName);
            string subject = "eFolding - Simulation";
            string body = "Simulation " + process.guid.ToString() + message;

            body += "</br>Direct access: " + string.Format("http://" + webServerName + "/Pages/SimulationAddEdit.aspx?guId={0}", process.guid.ToString());

            smtpClient.Send(to, subject, body);
        }
        catch (Exception ex)
        {
            GICO.WriteLine(ex);
        }
        finally
        {
            smtpClient = null;
        }
    }

    private void Edit()
    {
        Guid guid = new Guid(ViewState["guid"].ToString());

        using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        {
            Process process = ctx.Process.First(p => p.guid == guid);
            process.note = txtNote.Text.ToString().Trim();

            DataToProcess data = ctx.DataToProcess.First(p => p.process_guid == guid);

            data.isem = Convert.ToInt32(txtIsem.Text.Trim());
            data.loadDatFile = false;
            data.maxInterations = Convert.ToInt32(txtMaxInterations.Text.Trim());
            //data.numberOfDelta = Convert.ToDouble(txtNumberOfDelta.Text.Trim());
            data.totalSitio = Convert.ToInt32(txtTotalSitio.Text.Trim());

            //List<Model> model = ctx.Model.Where(p=>p.process_guid == guid).ToList<Model>();

            ctx.SaveChanges();
        }
        Response.Redirect("~/Pages/Simulation.aspx");

    }

    private void GetData()
    {
        Guid guid = new Guid(ViewState["guid"].ToString());

        using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        {
            Process temp = ctx.Process
                .Include("DataToProcess")
                .Include("Status").SingleOrDefault(p => p.guid == guid);

            txtIsem.Text = temp.DataToProcess.isem.ToString();
            txtMaxInterations.Text = temp.DataToProcess.maxInterations.ToString();

            txtTotalSitio.Text = temp.DataToProcess.totalSitio.ToString();
            txtTemperature.Text = temp.DataToProcess.temperature.ToString();

            txtNote.Text = temp.note;
            txtLabel.Text = temp.label;

            txtTemperature.Text = string.Empty;
            foreach (Model model in ctx.Model.Where(p => p.process_guid == guid).ToList<Model>())
            {
                txtModelHP.Text += model.value == 0 ? "H;" : "P;";
            }

            gvLog.DataSource = ctx.ProcessLog.Where(p => p.process_guid == guid).ToList();

            gvLog.DataBind();

            if (temp.StatusDescription != "created")
            {
                bttFinish.Visible = false;
            }

        }

        return;

    }
    private Guid GetUserID()
    {
        var currentUser = Membership.GetUser(User.Identity.Name);
        var userID = (Guid)currentUser.ProviderUserKey;
        return userID;
    }

    protected void txtMaxInterations_TextChanged(object sender, EventArgs e)
    {
        Interation();
    }

    private void Interation()
    {

        int maxInterations = Convert.ToInt32(txtMaxInterations.Text.Trim());
        int totalSitio = Convert.ToInt32(txtTotalSitio.Text.Trim());
        txtTime.Text = "Interations:" + Convert.ToInt32(maxInterations * 5 * totalSitio).ToString();

    }
    protected void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dd = (DropDownList)sender;
        int value = Convert.ToInt16(dd.SelectedValue);

        if (value == 3)
        {
            txtModelHP.Enabled = true;
        }
        else
        {
            txtModelHP.Enabled = false;
        }
        //if (value == 0 || value == 1 || value == 2)
        //{
        //    bttFinish.Text = "Finish";
        //    bttFinish.Enabled = true;
        //}
        //else if (value == 4 || value == 5)
        //{
        //    bttFinish.Text = "Finish";
        //    bttFinish.Enabled = false;
        //}
        //else
        //{
        //    bttFinish.Text = "Next";
        //    bttFinish.Enabled = true;
        //}
    }
    protected void txtBeta_TextChanged(object sender, EventArgs e)
    {

    }

    //protected void txtTemperature_TextChanged(object sender, EventArgs e)
    //{
    //    //if (Request["guid"] == null)
    //    //{

    //    //}
    //}
}