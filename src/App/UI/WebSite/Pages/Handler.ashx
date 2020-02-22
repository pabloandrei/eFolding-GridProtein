<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using GridProteinFolding.Middle.Helpers.CompressionHelpers;
using SIO = System.IO;
using GridProteinFolding.Middle.Helpers.TypesHelpers;

public class Handler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {

        Guid guid = new Guid(context.Request["guid"]);
        GetFile(context, guid);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void GetFile(HttpContext context, Guid guid)
    {
        //string dirBaseServer = System.AppConfigClient.ConfigurationManager.AppSettings["dirBaseServer"].ToString() + guid;
        string dirBaseWeb = System.Configuration.ConfigurationManager.AppSettings["dirBaseWeb"].ToString() + guid;
        string dirDownload = System.Configuration.ConfigurationManager.AppSettings["dirDownload"].ToString();

        if (!Directory.Exists(dirDownload))
            Directory.CreateDirectory(dirDownload);

        SIO.FileInfo sourceFile = new SIO.FileInfo(dirBaseWeb + ".zip");
        SIO.FileInfo destFileName = new SIO.FileInfo(dirDownload + sourceFile.Name);

        if (!File.Exists(destFileName.FullName))
        {
            //Compress FILE
            Compression.Compress(guid, sourceFile, new ExtendedDirectoryInfo(dirBaseWeb), true, string.Empty);

            if (File.Exists(destFileName.FullName))
                File.Delete(destFileName.FullName);

            File.Move(sourceFile.FullName, destFileName.FullName);
        }


        //return File(new ExtendedFileStream(destFileName.FullName.ToString(), FileMode.Open), "application/zip", guid + ".zip");

        Download(context, destFileName);

    }

    public void Download(HttpContext context, SIO.FileInfo file)
    {

        try
        {
            var path = file.FullName;

            context.Response.Clear();
            context.Response.AddHeader("Content-Length", file.Length.ToString());
            context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
            context.Response.TransmitFile(file.FullName);
            //context.Response.OutputStream.Write(fileContents, 0, fileContents.Length);
            context.Response.Flush();
            context.Response.End();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Source + " - " + ex.Message + " - " + ex.InnerException);
        }

    }


}