using System;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;

namespace GridProteinFolding.Core.eFolding.IO
{
    public static class Stream
    {
        public static class StateControl
        {
            public static Int64 numberOfLines = 0;
            public static Int32 splitFileEvery = 0;
            public static Int16 extensionfile = 0;
        }

        /// <summary>
        /// Metodo o qual efetua geração do arquivo texto com os Monomeros baseados em um dado Isem
        /// </summary>
        /// <param name="mList">Lista de Monomeros</param>
        /// <param name="breakLine">Pula linha entre as interações</param>
        /// <param name="overrideFile">Se o arquivo existir, o mesmo é sobreposto</param>
        public static void TrajectoryFile(long mCStep, long totalMoviments, long numberOfMovementsApplied, List<Structs.BasicStructs.Point> mList, Guid guid, Int32 splitFileEvery, bool firstStep = false)
        {
            StateControl.splitFileEvery = splitFileEvery;
            string fileName = "Trajectory" + Directory.FileExtension;

            StringBuilder output = new StringBuilder();

            ExtendedStreamWriter tw;
            string currentFileName = string.Empty;

            if (firstStep)
            {
                StateControl.numberOfLines = 0;
                tw = new ExtendedStreamWriter(Directory.SubDirTrajectorySet + @"\" + fileName, false, Config.CurrentGuid, Config.Crypt);
                output.Append("mCStep" + "\t");// + "totalMoviments" + "\t" + "NumberOfMovementsApplied" + "\t");

                for (int xFor = 0; xFor < mList.Count; xFor++)
                {
                    output.Append("x" + xFor + "\t" + "y" + xFor + "\t" + "z" + xFor);

                    if (xFor < (mList.Count - 1)) { output.Append("\t"); }

                    tw.Write(output.ToString());
                    output.Length = 0;
                }
                tw.WriteLine("");

            }
            else
            {
                if (StateControl.extensionfile == 0)
                {
                    currentFileName = fileName;
                }
                else
                {
                    currentFileName = "Trajectory" + StateControl.extensionfile + Directory.FileExtension;
                }

                tw = new ExtendedStreamWriter(Directory.SubDirTrajectorySet + @"\" + currentFileName, true, Config.CurrentGuid, Config.Crypt);

                StateControl.numberOfLines++;

                if (StateControl.numberOfLines == StateControl.splitFileEvery)
                {
                    StateControl.numberOfLines = 0;
                    StateControl.extensionfile++;
                }
            }


            tw.AutoFlush = true;

            //Layout MCS   M1x   M1y  M1z  M2x   M2y  M2z  M3x   M3y  M3z   ...  M27x   M27y  M27z

            output.Append(mCStep + "\t"); // + totalMoviments + "\t" + NumberOfMovementsApplied + "\t");
            tw.Write(output.ToString());
            output.Length = 0;

            for (int xFor = 0; xFor < mList.Count; xFor++)
            {
                output.Append(mList[xFor].x.ToString() + "\t" + mList[xFor].y.ToString() + "\t" + mList[xFor].z.ToString());

                if (xFor < (mList.Count - 1)) { output.Append("\t"); }

                tw.Write(output.ToString());
                output.Length = 0;
            }
            tw.WriteLine("");
            output = null;

            try
            {
                tw.Flush();
                tw.Close();
            }
            finally
            {
                tw = null;
            }
        }


        public static void TrajectoryFileSandBox(long mCStep, List<Structs.BasicStructs.Point> mList)
        {

            string fileName = "TrajectorySandBox" + Directory.FileExtension;

            StringBuilder output = new StringBuilder();

            ExtendedStreamWriter tw;
            string currentFileName = string.Empty;


            if (StateControl.extensionfile == 0)
            {
                currentFileName = fileName;
            }
            else
            {
                currentFileName = "Trajectory" + StateControl.extensionfile + Directory.FileExtension;
            }

            tw = new ExtendedStreamWriter(@"C:\" + @"\" + currentFileName, true, Config.CurrentGuid, Config.Crypt);

            tw.AutoFlush = true;

            //Layout MCS   M1x   M1y  M1z  M2x   M2y  M2z  M3x   M3y  M3z   ...  M27x   M27y  M27z

            output.Append(mCStep + "\t"); // + totalMoviments + "\t" + NumberOfMovementsApplied + "\t");
            tw.Write(output.ToString());
            output.Length = 0;

            for (int xFor = 0; xFor < mList.Count; xFor++)
            {
                output.Append(mList[xFor].x.ToString() + "\t" + mList[xFor].y.ToString() + "\t" + mList[xFor].z.ToString());

                if (xFor < (mList.Count - 1)) { output.Append("\t"); }

                tw.Write(output.ToString());
                output.Length = 0;
            }
            tw.WriteLine("");
            output = null;

            try
            {
                tw.Flush();
                tw.Close();
            }
            finally
            {
                tw = null;
            }
        }

        /// <summary>
        /// Metodo o qual efetua geração do arquivo texto com os Monomeros baseados em um dado Isem
        /// </summary>
        /// <param name="mList">Lista de Monomeros</param>
        /// <param name="breakLine">Pula linha entre as interações</param>
        /// <param name="overrideFile">Se o arquivo existir, o mesmo é sobreposto</param>
        public static void RecTrajectoryFileCalcSpinningRay_MIN(long mCStep, long totalMoviments, long numberOfMovementsApplied, List<Structs.BasicStructs.Point> mList, bool breakLine, Guid guid)
        {
            string fileName = GCPS.initialIsem + "_calcSpinningRay_MIN_" + GCPS.chain.contMoves.SumOfAcceptedAndRejectedMovement + Directory.FileExtension;
            bool append = false;
            ExtendedStreamWriter tw = new ExtendedStreamWriter(Directory.SubDirTrajectorySet + @"\" + fileName, append, Config.CurrentGuid, Config.Crypt);

            StringBuilder output = new StringBuilder();

            for (int xFor = 0; xFor < mList.Count; xFor++)
            {
                output.Append(mList[xFor].x.ToString() + "\t" + mList[xFor].y.ToString() + "\t" + mList[xFor].z.ToString());
                tw.Write(output.ToString());
                if (breakLine) { tw.WriteLine(); }
                output.Length = 0;
            }

            output = null;

            try
            {
                tw.Flush();
                tw.Close();
            }
            finally
            {
                tw = null;
            }
        }


        public static void StreamRecCalcSpinningRay(long sumOfAcceptedAndRejectedMovement, double calcSpinningRay, string fileName)
        {
            bool append = true;

            ExtendedStreamWriter tw = new ExtendedStreamWriter(Directory.SubDirTrajectorySet + @"\" + fileName, append, Config.CurrentGuid, Config.Crypt);

            StringBuilder output = new StringBuilder();

            output.Append(sumOfAcceptedAndRejectedMovement + "\t" + calcSpinningRay);
            tw.WriteLine(output.ToString());

            output = null;

            try
            {
                tw.Flush();
                tw.Close();
            }
            finally
            {
                tw = null;
            }
        }

        public static void RecMoveSet(Guid guid, string fileName)
        {
            bool append = false;

            StringBuilder output = new StringBuilder();
            output.Append("MOVES in GENERAL" + System.Environment.NewLine);
            output.Append("\tTotal Accept movements:\t" + GCPS.chain.contMoves.NumberOfMovementsApplied.ToString() + System.Environment.NewLine);
            output.Append("\tTotal Reject movements:\t" + GCPS.chain.contMoves.NumberOfMovementsRejected.ToString() + System.Environment.NewLine);
            output.Append("\tTotal attempts:\t" + GCPS.chain.contMoves.SumOfAcceptedAndRejectedMovement.ToString() + System.Environment.NewLine);
            output.Append("CRANKSHAFT" + System.Environment.NewLine);
            output.Append("\tcrankshaft 1 accepted:\t" + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftAccept_1 + System.Environment.NewLine);
            output.Append("\tcrankshaft 1 rejected:\t" + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftReject_1);
            output.Append("\t\t" + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftTotal_1 + System.Environment.NewLine);

            output.Append("\tcrankshaft 2 accepted:\t" + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftAccept_2 + System.Environment.NewLine);
            output.Append("\tcrankshaft 2 rejected:\t" + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftReject_2);
            output.Append("\t\t" + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftTotal_2 + System.Environment.NewLine);

            output.Append("\tcrankshaft 3 accepted:\t" + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftAccept_3 + System.Environment.NewLine);
            output.Append("\tcrankshaft 3 rejected:\t" + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftReject_3);
            output.Append("\t\t" + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftTotal_3 + System.Environment.NewLine);

            output.Append("\tTotal crankshaft Accept:\t" + GCPS.chain.contMoves.crankshaftAccept.ToString() + System.Environment.NewLine);
            output.Append("\tTotal crankshaft Reject:\t" + GCPS.chain.contMoves.crankshaftReject.ToString() + System.Environment.NewLine);
            output.Append("\tTotal crankshaft Attempts:\t" + GCPS.chain.contMoves.TotalCankshaftAttempts.ToString());
            output.Append("\t\t" + (GCPS.chain.contMoves.moveSetCrankshaft.crankshaftTotal_1 + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftTotal_2 + GCPS.chain.contMoves.moveSetCrankshaft.crankshaftTotal_3).ToString() + System.Environment.NewLine);


            output.Append("KINK" + System.Environment.NewLine);
            output.Append("\tTotal kink Accept:\t" + GCPS.chain.contMoves.kinkAccept.ToString() + System.Environment.NewLine);
            output.Append("\tTotal kink Reject:\t" + GCPS.chain.contMoves.kinkReject.ToString() + System.Environment.NewLine);
            output.Append("\tTotal kind Attempts:\t" + GCPS.chain.contMoves.TotatKinKAttempts.ToString() + System.Environment.NewLine);


            output.Append("END" + System.Environment.NewLine);
            output.Append("\tend 1 accepted:\t" + GCPS.chain.contMoves.moveSetEnd.endAccept_1 + System.Environment.NewLine);
            output.Append("\tend 1 rejected:\t" + GCPS.chain.contMoves.moveSetEnd.endReject_1);
            output.Append("\t\t" + GCPS.chain.contMoves.moveSetEnd.endTotal_1 + System.Environment.NewLine);

            output.Append("\tend 2 accepted:\t" + GCPS.chain.contMoves.moveSetEnd.endAccept_2 + System.Environment.NewLine);
            output.Append("end 2 rejected:\t" + GCPS.chain.contMoves.moveSetEnd.endReject_2);
            output.Append("\t\t" + GCPS.chain.contMoves.moveSetEnd.endTotal_2 + System.Environment.NewLine);

            output.Append("\tend 3 accepted:\t" + GCPS.chain.contMoves.moveSetEnd.endAccept_3 + System.Environment.NewLine);
            output.Append("\tend 3 rejected:\t" + GCPS.chain.contMoves.moveSetEnd.endReject_3);
            output.Append("\t\t" + GCPS.chain.contMoves.moveSetEnd.endTotal_3 + System.Environment.NewLine);

            output.Append("\tend 4 accepted:\t" + GCPS.chain.contMoves.moveSetEnd.endAccept_4 + System.Environment.NewLine);
            output.Append("\tend 4 rejected:\t" + GCPS.chain.contMoves.moveSetEnd.endReject_4);
            output.Append("\t\t" + GCPS.chain.contMoves.moveSetEnd.endTotal_4 + System.Environment.NewLine);

            output.Append("\tTotal ends Accept:\t" + GCPS.chain.contMoves.endsAccept.ToString() + System.Environment.NewLine);
            output.Append("\tTotal ends Reject:\t" + GCPS.chain.contMoves.endsReject.ToString() + System.Environment.NewLine);
            output.Append("\tTotal end Attempts:\t" + GCPS.chain.contMoves.TotalEndAttempts.ToString());
            output.Append("\t\t" + (GCPS.chain.contMoves.moveSetEnd.endTotal_1 + GCPS.chain.contMoves.moveSetEnd.endTotal_2 + GCPS.chain.contMoves.moveSetEnd.endTotal_3 + GCPS.chain.contMoves.moveSetEnd.endTotal_4).ToString() + System.Environment.NewLine);

            output.Append("STRETCHED" + System.Environment.NewLine);
            output.Append("\tstretched Accepted:\t0" + System.Environment.NewLine);
            output.Append("\tstretched Rejected:\t" + GCPS.chain.contMoves.othersReject.ToString() + System.Environment.NewLine);
            output.Append("\tTotal stretched Attempts:\t" + GCPS.chain.contMoves.othersReject.ToString() + System.Environment.NewLine);


            //GICO.WriteLine(guid, output.ToString());

            ExtendedStreamWriter tw = new ExtendedStreamWriter(Directory.SubDirTrajectorySet + @"\" + fileName, append, Config.CurrentGuid, Config.Crypt);

            tw.WriteLine(output.ToString());

            output = null;

            try
            {
                tw.Flush();
                tw.Close();
            }
            finally
            {
                tw = null;
            }
        }

        public static void StreamRecBlob(byte[] data)
        {
            bool append = true;
            string fileName = "blob" + DateTime.Now.ToShortDateString() + ".dat";
            ExtendedStreamWriter tw = new ExtendedStreamWriter(Directory.SubDirBlob + @"\" + fileName, append, Config.CurrentGuid, Config.Crypt);

            tw.Write(data);

            try
            {
                tw.Flush();
                tw.Close();
            }
            finally
            {
                tw = null;
            }
        }

        ///// <summary>
        ///// Save as coordenadas geradas no DeadEnd
        ///// </summary>
        //protected static void SaveDeadEnd(string file, string posDeadEnd)
        //{
        //    string dir =  Members.workSpace + @"\" + Resource.SubDirDeadEnd;

        //    string pathFile = dir + @"\" + file + Consts.extensionOfFile;

        ////    using (ExtentedStreamWriter sw = File.CreateText(pathFile))
        //ExtendedStreamWriter sw = new ExtendedStreamWriter();
        //    sw.CreateText(pathFile);
        //   // {
        //        int i = 0;
        //        foreach (Structs.Point temp in Members.monomero.r)
        //        {
        //            sw.WriteLine("{0}, {1}", i, temp.deadEndPoints);
        //            i++;
        //        }
        //    //}
        //sw.Flush();
        //    sw.Close();

        //}


        //protected static void ThreadStream(object type, object file)
        //{
        //    try
        //    {
        //        ThreadStart ts = new ThreadStart(type);
        //        Thread thread = new Thread(ts);
        //        thread.Start(file);
        //    }
        //    catch (Exception ex)
        //    {
        //        Thread.CurrentThread.Abort();
        //    }
        //}
    }
}
