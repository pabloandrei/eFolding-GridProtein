using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.ServiceModel;
using GridProteinFolding.UI.ProteinFoldingService.App;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.CompressionHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using SIO = System.IO;
using GridProteinFolding.Middle.Helpers.TypesHelpers;
using GridProteinFolding.UI.ProteinFoldingService.ServiceReference;
using System.Resources;
using GridProteinFolding.UI.ProteinFoldingService;
using GridProteinFolding.Middle.Helpers.Win32APIHelpers;

namespace ProteinFoldingService
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class FormProtein : System.Windows.Forms.Form, IDisposable
    {
        private static bool returnOfProcess = false;
        private static bool previousSimulation = false;
        private Thread baseThread = null;
        private System.Windows.Forms.TextBox txtBoxMessage;
        private Button start;
        private Button stop;
        private Button pause;
        private GroupBox threadManager;
        private IContainer components;
        private Label lblStatus;
        private System.Windows.Forms.Timer internalTimer;
        ResourceManager rm;

        public FormProtein()
        {

            InitializeComponent();

            OpenTimer();

            Base();

            baseThread = new Thread(new ThreadStart(Process));

            baseThread.IsBackground = true;

            rm = new ResourceManager("Resource", Assembly.GetExecutingAssembly());

            //PREPARA os DIRETORIOS

            Directory.CreateDirIfNotExist(new DirBaseService().dirBaseCliente);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            rm = null;

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProtein));
            this.txtBoxMessage = new System.Windows.Forms.TextBox();
            this.start = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.pause = new System.Windows.Forms.Button();
            this.threadManager = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.internalTimer = new System.Windows.Forms.Timer(this.components);
            this.threadManager.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBoxMessage
            // 
            this.txtBoxMessage.Location = new System.Drawing.Point(12, 12);
            this.txtBoxMessage.Multiline = true;
            this.txtBoxMessage.Name = "txtBoxMessage";
            this.txtBoxMessage.ReadOnly = true;
            this.txtBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxMessage.Size = new System.Drawing.Size(511, 251);
            this.txtBoxMessage.TabIndex = 1;
            this.txtBoxMessage.VisibleChanged += new System.EventHandler(this.txtBoxMessage_VisibleChanged);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(6, 21);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(90, 26);
            this.start.TabIndex = 7;
            this.start.Text = "Start";
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(198, 21);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(90, 26);
            this.stop.TabIndex = 8;
            this.stop.Text = "Stop";
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // pause
            // 
            this.pause.Location = new System.Drawing.Point(102, 21);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(90, 26);
            this.pause.TabIndex = 9;
            this.pause.Text = "Pause";
            this.pause.Click += new System.EventHandler(this.pause_Click);
            // 
            // threadManager
            // 
            this.threadManager.Controls.Add(this.lblStatus);
            this.threadManager.Controls.Add(this.pause);
            this.threadManager.Controls.Add(this.stop);
            this.threadManager.Controls.Add(this.start);
            this.threadManager.Location = new System.Drawing.Point(12, 269);
            this.threadManager.Name = "threadManager";
            this.threadManager.Size = new System.Drawing.Size(511, 58);
            this.threadManager.TabIndex = 7;
            this.threadManager.TabStop = false;
            this.threadManager.Text = "Thread Management";
            this.threadManager.Enter += new System.EventHandler(this.threadManager_Enter);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(294, 27);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStatus.Size = new System.Drawing.Size(67, 17);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Waiting...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // internalTimer
            // 
            this.internalTimer.Tick += new System.EventHandler(this.internalTimer_Tick);
            // 
            // FormProtein
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(535, 339);
            this.Controls.Add(this.threadManager);
            this.Controls.Add(this.txtBoxMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormProtein";
            this.Text = "Protein Folding Client";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.threadManager.ResumeLayout(false);
            this.threadManager.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show the system tray icon.					
            using (ProcessIcon pi = new ProcessIcon())
            {

                pi.Display();

                // Make sure the application runs!
                Application.Run();
            }
        }

        private void OpenTimer()
        {
            this.internalTimer.Interval = 1000;
            this.internalTimer.Enabled = true;
        }

        private void start_Click(object sender, System.EventArgs e)
        {
            ActionStart();
        }

        private void ActionStart()
        {

            internalTimer.Enabled = false;

            baseThread.Start();

            start.Enabled = false;
            stop.Enabled = true;
            pause.Enabled = true;

        }

        private void stop_Click(object sender, System.EventArgs e)
        {
            internalTimer.Enabled = true;

            baseThread.Abort();
            baseThread.Join();

            baseThread = new Thread(new ThreadStart(Process));

            baseThread.IsBackground = true;

            start.Enabled = true;
            stop.Enabled = false;
            pause.Enabled = false;
        }

        private void pause_Click(object sender, System.EventArgs e)
        {
            internalTimer.Enabled = true;

            if (0 != (baseThread.ThreadState & (ThreadState.Suspended | ThreadState.SuspendRequested)))
                baseThread.Resume();
            else
                baseThread.Suspend();
        }

        private void manageTime_CheckedChanged(object sender, System.EventArgs e)
        {
        }

        private void manageNumbers_CheckedChanged(object sender, System.EventArgs e)
        {
        }

        private void manageBoth_CheckedChanged(object sender, System.EventArgs e)
        {
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((baseThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                baseThread.Abort();

            baseThread = null;
        }

        private void threadManager_Enter(object sender, EventArgs e)
        {
        }

        #region BASE

        public void Base()
        {
            Sql.Process();
        }


        private static int timeForConsulting = Convert.ToInt16(ConfigurationManager.AppSettings["timeForConsulting"]);

        private void TestEcho()
        {
            //Test Connetion
            if (ProxyServiceClient.Echo())
            {
                string message = string.Empty;
                if (ProxyServiceClient.Autentication(ref message))
                {
                    DisplayService("Connected with server...", Color.Green);
                }
                else
                {
                    DisplayService("Erro connection with server...", Color.Red);
                    MessageBox.Show(message, "Error");
                }
            }
            else
            {
                DisplayService("Erro connection with server...", Color.Red);
            }
        }

        public void Process()
        {

            DisplayMessage("Protein Folding Service Started!");

            Sql.Open();

            while (true)
            {
                TestEcho();

                System.Threading.Thread.Sleep(1000 * timeForConsulting);

                CallServer(CallPendencies());
            }
        }

        private GridProteinFolding.Data.XMLData.Entity.Process CallPendencies()
        {
            GridProteinFolding.Data.XMLData.Entity.Process ret = Sql.SelectPend();
            if (ret != null)
            {
                previousSimulation = returnOfProcess = true;
            }
            else
            {
                previousSimulation = returnOfProcess = false;
            }

            return ret;
        }

        private bool CallServer(GridProteinFolding.Data.XMLData.Entity.Process process)
        {
            bool retRun = false;

            try
            {

                ServiceParamWcf paramWCF;

                bool oneProcessPeerGuid = false;
                //Se não existir pendência no cliente
                if (process == null)
                    paramWCF = ProxyServiceClient.GetOneProcess();
                else
                {
                    paramWCF = ProxyServiceClient.GetOneProcessGui(process.guid);
                    oneProcessPeerGuid = true;

                    //Implementar a mudança de STATUS - qdo ocorrer erro POS processamento = UPLOAD e etc
                    //variavel RET (estara dentro Run) tem q estar setada pra TRUE...senao dara ERRO 99...
                    //Proxy.SetStatus(paramWCF, BasicEnums.State.Upload);
                }

                if (paramWCF != null)
                {
                    ConsoleColor oldColour = Console.ForegroundColor;
                    GICO.ForegroundColor(ConsoleColor.Yellow);
                    DisplayMessage(string.Empty);
                    DisplayMessage(ExtendedString.Format("{0}> {1}", paramWCF.param.dataToProcess.Guid, Resource.Started));

                    GICO.ForegroundColor(oldColour);
                    if (oneProcessPeerGuid)
                        DisplayMessage(ExtendedString.Format("{0}> {1}", paramWCF.param.dataToProcess.Guid, Resource.GetOneProcess));
                    else
                        DisplayMessage(ExtendedString.Format("{0}> {1}", paramWCF.param.dataToProcess.Guid, Resource.GetOneProcessGui));

                    //Responde que esta sendo PROCESSADO     
                    DisplayMessage(ExtendedString.Format("{0}> {1}", paramWCF.param.dataToProcess.Guid, Resource.Running));

                    retRun = Run(paramWCF);

                    DisplayMessage(ExtendedString.Format("{0}> {1}", paramWCF.param.dataToProcess.Guid, Resource.Finished));
                    DisplayMessage(string.Empty);

                }

                return retRun;
            }
            catch (EndpointNotFoundException ex)
            {
                ConsoleColor oldColour = Console.ForegroundColor;
                GICO.ForegroundColor(ConsoleColor.Red);
                DisplayMessage(ex.Message);
                GICO.ForegroundColor(oldColour);

                new CustomLog().EndpointNotFoundException(ex);
            }

            return false;
        }

        //private void RecoverGuid(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam, BasicEnums.State state)
        //{
        //    Sql.RecoverGuid(newParam, state);
        //}

        //private void InsertGuid(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam, BasicEnums.State state)
        //{
        //    Sql.InsertGuid(newParam, state);
        //}

        //private GridProteinFolding.Data.XMLData.Entity.Process SelectGuid(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam)
        //{
        //    return Sql.SelectGuid(newParam);
        //}

        private GridProteinFolding.Data.XMLData.Entity.Process SetStatusProcessandoLocalDBToProcessing(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam, BasicEnums.State state)
        {
            return Sql.SetStatusProcessando(newParam, state);
        }

        private void UpdateStatus(GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam, BasicEnums.State state)
        {
            ProxyServiceClient.SetStatus(newParam, state);
        }


        private static byte Status(Guid guid)
        {
            return ProxyServiceClient.GetStatus(guid);
        }

        private bool Run(ServiceParamWcf paramWCF)
        {
            GridProteinFolding.Core.eFolding.Main objProcess = new GridProteinFolding.Core.eFolding.Main();

            //Executa REQUISICAO
            GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param();
            //Preparar os parametros para execução
            Param temp = paramWCF.param;
            GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param.PreperParams(ref temp, ref newParam);

            #region Atualiza Status local            
            if (Status(paramWCF.param.dataToProcess.Guid) == (int)BasicEnums.State.Processing)
                Sql.entityProcess = SetStatusProcessandoLocalDBToProcessing(newParam, BasicEnums.State.Processing);
            #endregion

            try
            {

                //if (Status(paramWCF.param.dataToProcess.Guid) == (int)BasicEnums.State.Waiting)
                //{
                //    DisplayMessage(ExtendedString.Format("{0}> {1}...", paramWCF.param.dataToProcess.Guid, BasicEnums.State.Waiting.ToString()));
                //    Sql.entityProcess = SetStatusProcessando(newParam, BasicEnums.State.Processing);
                //}

                if (Status(paramWCF.param.dataToProcess.Guid) == (int)BasicEnums.State.Processing)
                {
                    DisplayMessage(ExtendedString.Format("{0}> {1}...", paramWCF.param.dataToProcess.Guid, BasicEnums.State.Processing.ToString()));

                    Processing(ref objProcess, ref newParam, ref paramWCF);

                    if (returnOfProcess)
                        UpdateStatus(newParam, BasicEnums.State.Processed);
                    else
                    {
                        UpdateStatus(newParam, BasicEnums.State.Error);
                        DisplayMessage(ExtendedString.Format("{0}> {1}...", paramWCF.param.dataToProcess.Guid, BasicEnums.State.Error.ToString()));
                    }
                }

                if (Status(paramWCF.param.dataToProcess.Guid) == (int)BasicEnums.State.Processed)
                {
                    DisplayMessage(ExtendedString.Format("{0}> {1}...", paramWCF.param.dataToProcess.Guid, BasicEnums.State.Processed.ToString()));
                    UpdateStatus(newParam, returnOfProcess == true ? BasicEnums.State.Upload : BasicEnums.State.Error);
                    //System.Threading.Thread.Sleep(1000);
                }

                if (Status(paramWCF.param.dataToProcess.Guid) == (int)BasicEnums.State.Upload)
                {
                    DisplayMessage(ExtendedString.Format("{0}> {1}...", paramWCF.param.dataToProcess.Guid, (BasicEnums.State.Upload.ToString())));
                    returnOfProcess = true;
                    //Upload dos resultados
                    Upload(paramWCF);
                    //Atualiza Status
                    UpdateStatus(newParam, returnOfProcess == true ? BasicEnums.State.BULK : BasicEnums.State.Error);
                    //System.Threading.Thread.Sleep(500);
                }

                if (Status(paramWCF.param.dataToProcess.Guid) == (int)BasicEnums.State.ClearTempClient)
                {
                    if (AppConfigClient.Param == null)
                    {
                        AppConfigClient.Param = newParam;
                    }
                    //Limpar folder cliente utilizado no processamento
                    new GridProteinFolding.Core.eFolding.IO.Directory().ClearProcessClientFolder();

                    //Atualiza Status
                    UpdateStatus(newParam, returnOfProcess == true ? BasicEnums.State.ClearTempServer : BasicEnums.State.Error);
                    DisplayMessage(ExtendedString.Format("{0}> {1}...", paramWCF.param.dataToProcess.Guid, returnOfProcess == true ? BasicEnums.State.ClearTempServer.ToString() : BasicEnums.State.Error.ToString()));
                }

                GICO.WriteLine(ExtendedString.Format("{0}> {1}...", paramWCF.param.dataToProcess.Guid, "Process of simulation finished!"));


                return returnOfProcess;
            }
            catch (Exception ex)
            {
                new CustomLog().Exception(ex);

                UpdateStatus(newParam, BasicEnums.State.Error);

                return false;
            }
            //finally
            //{
            //    GridProteinFolding.Data.SQLite.SQLite.CloseDatabase();
            //}

        }

        void Processing(ref GridProteinFolding.Core.eFolding.Main objProcess,
            ref GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param newParam,
            ref ServiceParamWcf paramWCF)
        {
            //PROCESS MAIN
            returnOfProcess = objProcess.Process(ref newParam);
            objProcess = null;
        }

        void Upload(ServiceParamWcf paramWCF)
        {
            try
            {
                Guid guid = paramWCF.param.dataToProcess.Guid;

                //string path = ConfigurationManager.AppSettings["dirBaseClient"].ToString() + guid;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\efolding.fcfrp.usp.br\Client\" + guid;
                //string destPath = ConfigurationManager.AppSettings["uploadFolder"].ToString();
                string destPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\efolding.fcfrp.usp.br\Client\_temp\" + guid;

                SIO.FileInfo sourceFile = new File().FileInfos(path + ".zip");
                SIO.FileInfo destFileName = new File().FileInfos(destPath + ".zip");

                //Valida se arquivo não existe, de uma tentativa de enviado anterior
                if (previousSimulation == false) //!File.Exists(destFileName.FullName) && 
                {

                    //Compress FILE
                    //GICO.WriteLine(guid, rm.GetString("UploadCompression"));
                    GICO.WriteLine(guid, "UploadCompression");
                    Compression.Compress(guid, sourceFile, new ExtendedDirectoryInfo(path), true, string.Empty);

                    if (File.Exists(destFileName.FullName))
                        File.Delete(destFileName.FullName);

                    //GICO.WriteLine(guid, rm.GetString("UploadFileMove"));
                    GICO.WriteLine(guid, "UploadFileMove");
                    File.Move(sourceFile.FullName, destFileName.FullName);
                }

                GICO.WriteLine(guid, ExtendedString.Format("Upload file {0}..", destFileName.Name));

                ProxyDocumentManagmentClient.DocumentEcho(destFileName);
                using (System.IO.BinaryReader fs = new System.IO.BinaryReader(System.IO.File.Open(destFileName.FullName, System.IO.FileMode.Open)))
                {
                    int pos = 0;
                    bool append = false;
                    int length = (int)fs.BaseStream.Length;

                    while (pos < length)
                    {
                        byte[] bytes = fs.ReadBytes(1024 * 1024);
                        ProxyDocumentManagmentClient.UploadDocument(destFileName, bytes, append);

                        append = true;
                        pos += 1024 * 1024;
                    }
                }

                //Extract
                GICO.WriteLine(guid, ExtendedString.Format("Extract file {0}..", destFileName.Name));
                ProxyDocumentManagmentClient.ExtractDocument(destFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DisplayMessage(string message)
        {
            txtBoxMessage.SafeInvoke(d => d.Text += message + Environment.NewLine);
        }

        public void DisplayService(string message, Color color)
        {
            lblStatus.SafeInvoke(d => d.ForeColor = color);
            lblStatus.SafeInvoke(d => d.Text = message);
        }

        public void SetText(string test1, string test2)
        {
        }
        #endregion

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void internalTimer_Tick(object sender, EventArgs e)
        {
            if (Win32.GetIdleTime() > 8000)
            {
                ActionStart();
            }
        }

        private void txtBoxMessage_VisibleChanged(object sender, EventArgs e)
        {
            if (txtBoxMessage.Visible)
            {
                txtBoxMessage.SelectionStart = txtBoxMessage.TextLength;
                txtBoxMessage.ScrollToCaret();
            }
        }
    }
}
