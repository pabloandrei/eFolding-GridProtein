<%@ Application Language="C#" %>

<script RunAt="server">

    private static readonly log4net.ILog Log = log4net.LogManager.GetLogger("Global");

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application.Add("webServerName", System.Configuration.ConfigurationManager.AppSettings["webServerName"].ToString());

        log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.AppSettings["log4net.Config"]));
        log4net.ILog logger = log4net.LogManager.GetLogger("File");
        Log.Info("Startup application.");
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
        Exception ex = Server.GetLastError().GetBaseException();
        Log.Error("App_Error", ex);

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        log4net.ILog logger = log4net.LogManager.GetLogger("File");
        Log.Error("Startup application.");

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
