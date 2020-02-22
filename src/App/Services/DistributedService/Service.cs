using System;
using System.Collections.Generic;
using System.Linq;
using GridProteinFolding.Entities.Internal;
using GridProteinFolding.Entities.Membership;
using System.Configuration;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using System.Net.Mail;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Middle.Helpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;
/*
using System.Text;
using GridProteinFolding.Middle.Helpers.NetworkHelpers;
using GridProteinFolding.Enums;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;
*/

[assembly: CLSCompliant(true)]
namespace GridProteinFolding.Services.DistributedService
{
    public class Service : IDisposable
    {

        public Service()
        {

        }
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        public static string Echo()
        {
            return "echo";
        }

        private static void ParseIntegration(ref Process tempProcess)
        {
            if (tempProcess.Model.Count == 0) //tempProcess.Model.Count == null || 
            {
                throw new System.Exception("Model is null or count equal zero!");
            }

            if (tempProcess.Output == null)
            {
                throw new System.Exception("Output is null or count equal zero!");
            }

        }

        public static bool Autentication(RequestorInfo requestorInfo, ref string message)
        {
            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                ClientCurrentVersion clientCurrentVersion = ctx.ClientCurrentVersion.FirstOrDefault();
                if (clientCurrentVersion.currentVersion.Equals(requestorInfo.clientVersion))
                {
                    return true;
                }
                else
                {
                    message = "ERROR: Client version is not valid. You must make the update through the version available at http://efolding.fcfrp.usp.br/App/";
                    return false;
                }
            }
        }


        public static Process GetOneProcess(RequestorInfo requestorInfo)
        {

            Process tempProcess = new Process();

            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                tempProcess = ctx.Process.Include("DataToProcess").Include("DataToResult").Include("Model").Include("ConfigApp").Include("Output").FirstOrDefault(p => p.status_id == 1); // || p.status_id == 3 || p.status_id == 4);

                try
                {
                    if (tempProcess != null)
                    {
                        //TRATATIVA de INTEGRIDADE dos DADOS
                        ParseIntegration(ref tempProcess);
                        //SETA STATUS, se possível
                        SetStatus(ref tempProcess, ref requestorInfo);
                    }
                    return tempProcess;
                }
                catch (Exception ex)
                {
                    GICO.WriteLine(String.Format("{0}> {1} ({2} {3})", tempProcess.guid.ToString(), DateTime.Now, "Exception:", ex.Message.ToString()));

                    BasicEnums.State newstatus = BasicEnums.State.Error;
                    SetOneProcess(tempProcess.guid, newstatus, requestorInfo);

                    return null;
                }
            }
        }

        private static void SetStatus(ref Process tempProcess, ref RequestorInfo requestorInfo)
        {

            if (tempProcess != null)
            {
                BasicEnums.State newstatus = NewStatus(tempProcess.Status);
                if (newstatus != BasicEnums.State.NoChangeStatus)
                    SetOneProcess(tempProcess.guid, newstatus, requestorInfo);
            }
        }

        private static BasicEnums.State NewStatus(Status status)
        {
            if (status.id == Convert.ToByte(BasicEnums.State.Waiting))
            {
                return BasicEnums.State.Processing;
            }
            else
                return BasicEnums.State.NoChangeStatus;

        }

        public static Process GetOneProcessGui(Guid gui, RequestorInfo requestorInfo)
        {
            Process tempProcess = new Process();

            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                tempProcess = ctx.Process.Include("DataToProcess").Include("DataToResult").Include("Model").Include("ConfigApp").Include("Output").FirstOrDefault(p => p.guid == gui && p.macAddr == requestorInfo.macAddr);

                try
                {
                    if (tempProcess != null)
                    {
                        //TRATATIVA de INTEGRIDADE dos DADOS
                        ParseIntegration(ref tempProcess);
                        //SETA STATUS, se possível
                        SetStatus(ref tempProcess, ref requestorInfo);
                    }
                    return tempProcess;
                }
                catch (Exception ex)
                {
                    GICO.WriteLine(String.Format("{0} {1} ({2} {3})", tempProcess.guid.ToString(), DateTime.Now, "Exception:", ex.Message.ToString()));

                    BasicEnums.State newstatus = BasicEnums.State.Error;
                    SetOneProcess(tempProcess.guid, newstatus, requestorInfo);

                    return null;
                }
            }


        }


        public static Process GetOneProcessGuiForETL(Guid gui, Services.DistributedService.RequestorInfo requestorInf)
        {

            Process tempProcess = new Process();

            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                tempProcess = ctx.Process.Include("DataToProcess").Include("DataToResult").Include("Model").Include("ConfigApp").Include("Output").FirstOrDefault(p => p.guid == gui);

                try
                {
                    if (tempProcess != null)
                    {
                        //TRATATIVA de INTEGRIDADE dos DADOS
                        ParseIntegration(ref tempProcess);
                        GICO.WriteLine(gui, String.Format("{0} {1} @ {2}({3})", DateTime.Now, "Processing ETL", requestorInf.macAddr, requestorInf.machineName));
                    }
                    return tempProcess;
                }
                catch (Exception ex)
                {
                    GICO.WriteLine(String.Format("{0} {1} ({2} {3})", tempProcess.guid.ToString(), DateTime.Now, "Exception:", ex.Message.ToString()));

                    BasicEnums.State newstatus = BasicEnums.State.ETLError;
                    SetOneProcess(tempProcess.guid, newstatus, requestorInf);

                    return null;
                }
            }
        }

        public static List<Guid> GetGuidsToApplications()
        {

            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                List<Guid> guid = ctx.Process.Where(p => p.status_id == 8)
                    .Select(p => p.guid)
                    .ToList<Guid>();

                return guid;
            }

        }

        public static byte GetStatus(Guid guid)
        {

            Process temp = new Process();

            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                temp = ctx.Process.FirstOrDefault(p => p.guid == guid);
                if (temp != null)
                {
                    return temp.status_id;
                }
                else
                {
                    return 0;
                }
            }

        }

        //public static Entities.Internal.ConfigApp GetConfigApp()
        //{

        //    Entities.Internal.ConfigApp temp = new Entities.Internal.ConfigApp();

        //    using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        //    {
        //        temp = ctx.ConfigApp.FirstOrDefault();
        //    }

        //    return temp;
        //}


        //public static void RunMonteC(Guid guid)
        //{
        //    using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        //    {
        //        GridProteinFolding.Entities.Internal.DataToProcess temp = ctx.DataToProcess.FirstOrDefault(p => p.process_guid == guid);

        //        temp.runMonteC = true;

        //        ctx.SaveChanges();
        //    }
        //}


        //public static void SetUFile(Param param)
        //{
        //    SetUFile(param.dataToProcess.Guid, param.files.Debug);
        //}

        //public static void SetUFile(Guid guid, string Ufile)
        //{
        //    using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        //    {
        //        GridProteinFolding.Entities.Internal.Files temp = ctx.Files.FirstOrDefault(p => p.guid == guid);
        //        if (temp != null)
        //            temp.UFile = Ufile;
        //        else
        //        {
        //            GridProteinFolding.Entities.Internal.Files files = new GridProteinFolding.Entities.Internal.Files();
        //            files.guid = guid;
        //            files.UFile = Ufile;
        //            ctx.AddToFiles(files);
        //        }
        //        ctx.SaveChanges();
        //    }
        //}

        private static void Expurg(Guid processGuid, GridProteinFoldingEntities ctx)
        {

            ////List<Seed> seed = ctx.Seed.Where(p => p.process_guid == processGuid).ToList<Seed>();

            ////if (seed != null & seed.Count > 0)
            ////{
            ////int seedId = seed[0].isem;
            ////List<Histogram> histogram = ctx.Histogram.Where(p => p.seed_isem == seedId).ToList<Histogram>();

            ////if (histogram != null)
            ////{
            ////    foreach (Histogram item in histogram)
            ////    {
            ////        Frequencies frequencies = ctx.Frequencies.FirstOrDefault(p => p.histogram_id == item.id);
            ////        ctx.DeleteObject(frequencies);

            ////        ctx.DeleteObject(histogram);
            ////    }
            ////}

            ////List<Neighbors> neighbors = ctx.Neighbors.Where(p => p.seed_isem == seedId).ToList<Neighbors>();
            ////if (neighbors != null && neighbors.Count > 0)
            ////{
            ////    ctx.DeleteObject(neighbors);
            ////}

            ////foreach (Seed item in seed)
            ////{
            ////    ctx.DeleteObject(item);
            ////}

            //ProcessLog processLog = ctx.ProcessLog.FirstOrDefault(p => p.process_guid == processGuid);
            //ctx.DeleteObject(processLog);

            //ctx.SaveChanges();
            //// }
        }


        public static bool SetBlob(Guid guid, byte[] data)
        {

            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                Blob temp = ctx.Blob.FirstOrDefault(p => p.process_guid == guid);

                if (temp == null)
                {
                    temp.blob1 = data;
                }
                else
                {
                    temp.process_guid = guid;
                    temp.blob1 = data;
                    ctx.Blob.Add(temp);
                }

                ctx.SaveChanges();
            }

            return true;
        }

        public static byte SetOneProcess(Guid guid, BasicEnums.State state, RequestorInfo requestorInfo)
        {
            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                Process temp = ctx.Process.FirstOrDefault(p => p.guid == guid);
                temp.status_id = Convert.ToByte(state);
                temp.macAddr = requestorInfo.macAddr;
                temp.machineName = requestorInfo.machineName;
                ctx.SaveChanges();

                Condicional(guid, ctx, temp, ref state, ref requestorInfo);
            }

            return GetStatus(guid);
        }

        public static byte SetOneProcess(Param param, BasicEnums.State state, RequestorInfo requestorInfo)
        {
            Guid guid = param.dataToProcess.Guid;
            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                Process temp = ctx.Process.FirstOrDefault(p => p.guid == guid);
                temp.status_id = Convert.ToByte(state);
                temp.macAddr = requestorInfo.macAddr;
                temp.machineName = requestorInfo.machineName;
                ctx.SaveChanges();

                Condicional(guid, ctx, temp, ref state, ref requestorInfo);
            }

            return GetStatus(guid);
        }

        private static void Condicional(Guid guid, GridProteinFoldingEntities ctx, Process temp, ref BasicEnums.State state, ref RequestorInfo requestorInfo)
        {
            aspnet_Users user = new GridProteinFolding_MemberShipEntities().aspnet_Users.FirstOrDefault(u => u.UserId == temp.userId);

            if (state == BasicEnums.State.Processing)
            {
                GICO.WriteLine(guid, String.Format("{0} {1} @ {2}({3})", DateTime.Now, "Processing", temp.macAddr, temp.machineName));

                string message = " was started!";

                if (temp.emailNotification == Convert.ToByte(BasicEnums.EmailNotification.Enviar))
                {
                    SendMail(ctx, guid, message, user);
                }

                Expurg(guid, ctx);

            }
            if (state == BasicEnums.State.BULK)
            {
                #region BULK
                GICO.WriteLine(guid, String.Format("{0} {1} @ {2}({3})", DateTime.Now, "BULK", temp.macAddr, temp.machineName));
                //ExtendedDirectoryInfo dirBaseServer = new ExtendedDirectoryInfo(ConfigurationManager.AppSettings["dirBaseServer"].ToString());

                //object[] parameters = new object[2];
                //parameters[0] = dirBaseServer + @"\" + guid + @"\Seed";
                //parameters[1] = guid.ToString();

                //ctx.ExecuteStoreCommand("EXEC dbo.BunkFiles {0},{1}", parameters);

                state = BasicEnums.State.ClearTempClient;
                temp.status_id = Convert.ToByte(state);
                ctx.SaveChanges();
                #endregion

                #region ClearTempServer
                GICO.WriteLine(guid, String.Format("{0} {1} @ {2}({3})", DateTime.Now, "ClearTempServer", temp.macAddr, temp.machineName));
                DeleteFileServer_temp(guid);

                state = BasicEnums.State.Finalized;
                temp.status_id = Convert.ToByte(state);
                ctx.SaveChanges();
                #endregion
            }

            //if (state == Enums.BasicEnums.State.Excel)
            //{

            //    Applications objApplications = new Applications();

            //    GICO.Write("Excel..");
            //    objApplications.Excel(param);

            //    GICO.WriteLine("{0}:{1}", param.dataToProcess.Guid.ToString(), "DONE!");
            //    objApplications = null;

            //    state = Enums.BasicEnums.State.Origin;
            //    temp.status_id = Convert.ToByte(state);
            //    ctx.SaveChanges();
            //}

            //if (state == Enums.BasicEnums.State.Origin)
            //{
            //    ProcessData(param.dataToProcess.Guid);

            //    Applications objApplications = new Applications();

            //    GICO.Write("Origin..");
            //    objApplications.Origin(param.dataToProcess.Guid);

            //    GICO.WriteLine("{0}:{1}", param.dataToProcess.Guid.ToString(), "DONE!");
            //    objApplications = null;

            //    state = Enums.BasicEnums.State.ClearTempClient;
            //    temp.status_id = Convert.ToByte(state);
            //    ctx.SaveChanges();
            //}

            //if (state == BasicEnums.State.ClearTempServer)
            //{
            //    GICO.WriteLine(guid, String.Format("{0} {1}:", DateTime.Now, "ClearTempServer.."));
            //    DeleteFileServer_temp(guid);

            //    state = BasicEnums.State.Finalized;
            //    temp.status_id = Convert.ToByte(state);
            //    ctx.SaveChanges();
            //}

            //if (state == Enums.BasicEnums.State.Decrypt)
            //{
            //    Applications objApplications = new Applications();
            //    GICO.Write("Decrypt..");
            //    objApplications.Decrypt(param);
            //    GICO.WriteLine("{0}:{1}", param.dataToProcess.Guid.ToString(), "DONE!");
            //    objApplications = null;

            //    state = Enums.BasicEnums.State.Web;
            //    temp.status_id = Convert.ToByte(state);
            //    ctx.SaveChanges();
            //}

            //if (state == Enums.BasicEnums.State.Web)
            //{

            //    Applications objApplications = new Applications();
            //    GICO.Write("Web..");
            //    objApplications.Web(param);
            //    GICO.WriteLine("{0}:{1}", param.dataToProcess.Guid.ToString(), "DONE!");
            //    objApplications = null;

            //    state = Enums.BasicEnums.State.Finalized;
            //    temp.status_id = Convert.ToByte(state);
            //    ctx.SaveChanges();                
            //}

            if (state == BasicEnums.State.Finalized)
            {
                GICO.WriteLine(guid, String.Format("{0} {1} @ {2}({3})", DateTime.Now, "Finalized", temp.macAddr, temp.machineName));

                if (temp.emailNotification == Convert.ToByte(BasicEnums.EmailNotification.Enviar))
                {
                    string message = "was completed!";
                    SendMail(ctx, guid, message, user);
                }
            }
        }


        private static void DeleteFileServer_temp(Guid guid)
        {
            Directory.DeleteFileAndDirIfExists(ConfigurationManager.AppSettings["uploadFolder"].ToString(), guid);
        }

        private static bool SendMail(GridProteinFoldingEntities ctx, Guid guid, string message, aspnet_Users user)
        {
            //Sent e-mail
            Middle.Helpers.NetworkHelpers.SmtpClient smtpClient = new Middle.Helpers.NetworkHelpers.SmtpClient();
            Process process = ctx.Process.FirstOrDefault(p => p.guid == guid);


            if (!(process.emailNotification == Convert.ToByte(BasicEnums.EmailNotification.Enviar)))
            {
                try
                {

                    System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(user.aspnet_Membership.LoweredEmail, user.UserName);
                    string subject = "eFolding - Simulation";
                    string body = "Simulation " + process.guid.ToString() + message;
                    body += "Direct access: efolding.fcfrp.usp.br/Pages/GCPS.aspx?guid= " + process.guid.ToString();

                    if (smtpClient.Send(to, subject, body))
                        return true;
                    else
                        return false;

                }
                catch (SmtpException ex)
                {
                    new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().SmtpException(ex, Types.ErrorLevel.Warning);
                    return false;
                }
                catch (Exception ex)
                {
                    GICO.WriteLine(ex);
                    throw;
                }
                finally
                {
                    smtpClient = null;
                }
            }
            else
            {
                return true;
            }
        }

        //private static void DeleteSeed(Guid guid)
        //{

        //    using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        //    {
        //        var model = ctx.Seed.FirstOrDefault(f => f.process_guid == guid);
        //        if (model != null)
        //        {
        //            ctx.DeleteObject(model);

        //            ctx.SaveChanges();
        //        }

        //    }

        //}


        //private static void ProcessData(Guid guid)
        //{

        //    string dirBaseServer = ConfigurationManager.AppSettings["dirBaseServer"].ToString() + @"\" + guid.ToString();
        //    string debug = "Debug";

        //    string dirSeed = dirBaseServer + @"\" + debug + @"\" + Resource.DirSeed;
        //    //string dirCoord = dirBaseServer + @"\" + Resource.DirCoord;
        //    //string dirClassificationOfMotion = dirBaseServer + @"\" + Resource.DirClassificationOfMotion;
        //    //string dirServerNeighbors = dirBaseServer + @"\" + Resource.DirNeighbors;
        //    //string dirDeadEnd = dirBaseServer + @"\" + Resource.DirDeadEnd;
        //    //string dirServerHistogram = dirBaseServer + @"\" + Resource.DirHistogram;

        //    //Deleta previamente, se o registro existir
        //    DeleteSeed(guid);


        //    int count = 0;

        //    using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        //    {
        //        //IF EXIST, RUN...
        //        if (Directory.Exists(dirSeed))
        //        {
        //            foreach (string fileEntries in Directory.GetFiles(dirSeed))
        //            {
        //                Seed seed = new Seed();

        //                //var fs = new ExtendedFileStream(fileEntries, FileMode.Open, FileAccess.Read);

        //                using (var sr = new ExtendedStreamReader(fileEntries, Config.CurrentGuid, true))
        //                {
        //                    string line = string.Empty;
        //                    line = sr.ReadLine();

        //                    seed.isem = Convert.ToInt32(line.Trim());

        //                }
        //                //fs.Flush();
        //                //fs.Close();

        //                seed.process_guid = guid;

        //                ctx.AddToSeed(seed);

        //                if ((count % 100) == 0)
        //                {
        //                    ctx.SaveChanges();
        //                }
        //                count++;
        //                seed = null;
        //            }


        //            ctx.SaveChanges();
        //        }
        //        else
        //        {
        //            GICO.WriteLine("Directory not found: {0}, Integrator of ORIGIN not executed...", dirSeed);
        //        }
        //    }
        //}


        //public static RemoteEndpointMessageProperty endPointClient()
        //{

        //    OperationContext context = OperationContext.Current;

        //    MessageProperties messageProperties = context.IncomingMessageProperties;

        //    RemoteEndpointMessageProperty endpointProperty =
        //        messageProperties[RemoteEndpointMessageProperty.Name]
        //        as RemoteEndpointMessageProperty;

        //    return endpointProperty;
        //}


    }
}
