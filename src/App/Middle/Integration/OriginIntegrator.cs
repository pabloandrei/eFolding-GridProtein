using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Origin;
using System.Configuration;
using GridProteinFolding.Entities;
using GridProteinFolding.Entities.Internal;
using System.Runtime.InteropServices;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using SIO = System.IO;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;

namespace GridProteinFolding.Middle.Integration
{
    // REFERENCIA: http://www.originlab.com/doc/Orglab/Orglab
    // REFERENCIA UTILIZADA: C:\Program Files\OriginLab\Origin2017
    // Pos instalacao ORIGIN 2017
    // ADD Reference: Origin8.tlb

    public class OriginIntegrator
    {
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private Param param;

        public OriginIntegrator(Param param)
        {
            this.param = param;
        }


        static SIO.DirectoryInfo dirBaseServer = new SIO.DirectoryInfo(ConfigurationManager.AppSettings["dirBaseServer"].ToString());
        static string dirOrigin = dirBaseServer.FullName + Resource.SubDirOrigin;

        private double pointX, pointY, pointZ;
        private Origin.ApplicationSI pOrigin;
        private Origin.Worksheet pWorksheet;
        private Origin.GraphLayer pGraphLayer;
        private Origin.DataPlot pDataPlot;


        public void Execute()
        {
            GICO.WriteLine("OriginConnect: Starting..");
            Init(true, param);
        }


        //public void Execute(Seed seed)
        //{
        //    GICO.WriteLine("OriginConnect: Starting..");
        //    Init(true, seed);
        //}


        //private void Init(bool showDialogBox, Guid guid)
        //{
        //    using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
        //    {
        //        List<Seed> seeds = ctx.Seed.Where(s => s.process_guid == guid).ToList();
        //        foreach (Seed seed in seeds)
        //        {
        //            Init(showDialogBox, seed);
        //        }

        //    }

        //}

        private void Init(bool showDialogBox, Param param)
        {
            Start(showDialogBox);
            OpenSheet(param);
            Save(param);
            Close(param);
            System.Threading.Thread.Sleep(5000);
        }

        private void Start(bool showDialogBox)
        {
            try
            {
                pOrigin = new ApplicationSI();
            }
            catch (System.Exception ex)
            {
                String strMsg = ResourceException.Origin_FailedPlotting;
                GICO.WriteLine(strMsg);
                GICO.WriteLine(ex);
                return;
            }

            //Show o DIALOG BOX do ORIGIN
            if (showDialogBox)
                pOrigin.Visible = MAINWND_VISIBLE.MAINWND_SHOW;



        }

        private void OpenSheet(Param param)
        {
            try
            {
                String strWkName = "ScatterData";
                pWorksheet = pOrigin.FindWorksheet(strWkName);
                if (pWorksheet == null) //if check don't exist, create workbook
                    strWkName = (String)pOrigin.CreatePage((int)Origin.PAGETYPES.OPT_WORKSHEET, strWkName, "Origin", 2);
                pWorksheet = (Worksheet)pOrigin.WorksheetPages[strWkName].Layers[0];

                //prepare XYZ column for putting data
                pWorksheet.Cols = 3;

                Column colA = pWorksheet.Columns[0];
                colA.Type = COLTYPES.COLTYPE_X;

                Column colB = pWorksheet.Columns[1];
                colB.Type = COLTYPES.COLTYPE_Y;

                Column colC = pWorksheet.Columns[2];
                colC.Type = COLTYPES.COLTYPE_Z;

                //prepare graph layer for plotting
                String strGrName = "3DScatterPlot"; //if check don't exist, create it.
                pGraphLayer = pOrigin.FindGraphLayer(strGrName);
                if (pGraphLayer == null)
                    strGrName = (String)pOrigin.CreatePage((int)Origin.PAGETYPES.OPT_GRAPH, strGrName, "3D", 2); //3D scatter need "3D" template
                pGraphLayer = (GraphLayer)pOrigin.GraphPages[strGrName].Layers[0];

                //parepare XYZDataRange
                DataRange drXYZ = pWorksheet.NewDataRange(0, 0, -1, -1);

                //plot 3D scatter on GraphLayer
                String strPlotName = "ScatterData_C"; //if exists, don't add new plot, plot name is related to workbook name and column name.

                pDataPlot = pGraphLayer.DataPlots[strPlotName];
                if (pDataPlot == null)
                {
                    pDataPlot = pGraphLayer.DataPlots.Add(drXYZ, PLOTTYPES.IDM_PLOT_3D_SCATTER);
                }

                //force axis rescale as auto to make plot visible
                pGraphLayer.Execute("layer.x.rescale=3");
                pGraphLayer.Execute("layer.y.rescale=3");
                pGraphLayer.Execute("layer.z.rescale=3");

                SentData(param);
            }
            catch (System.Exception ex)
            {
                String strMsg = ResourceException.Origin_FailedPlotting;
                GICO.WriteLine(strMsg);
                GICO.WriteLine(ex);
                throw;
            }
        }

        private void SentData(Param param)
        {
            GICO.WriteLine("OriginConnect: Salving points of Seed {0}..", param.dataToProcess.Guid.ToString());
            //Protein.LoadSeedCoord(fileReader, true);
            using (GridProteinFoldingEntities ctx = new GridProteinFoldingEntities())
            {
                List<Entities.Internal.Configuration> configuration = ctx.Configuration.Where(c => c.process_guid == param.dataToProcess.Guid && c.mcStep == 0).ToList();

                foreach (Entities.Internal.Configuration coordinate in configuration)
                {
                    int nFactor = pWorksheet.Rows;
                    pointX = coordinate.x;
                    pointY = coordinate.y;
                    pointZ = coordinate.z;

                    double[,] Data = new double[1, 3];
                    Data[0, 0] = pointX;
                    Data[0, 1] = pointY;
                    Data[0, 2] = pointZ;
                    pWorksheet.SetData(Data, -1, 0); //append              

                }
            }
        }

        private void Save(Param param)
        {
            GICO.WriteLine("OriginConnect: Salving fileReader..");
            if (pOrigin != null)
            {
                if (!Directory.Exists(dirOrigin))
                {
                    Directory.CreateDirectory(dirOrigin);
                }

                string pathFile = dirOrigin + @"\" + param.dataToProcess.Guid.ToString() + Resource.OriginProjectFile;

                if (File.Exists(pathFile))
                    File.Delete(pathFile);

                pOrigin.Save(pathFile);
            }
        }

        private void Close(Param param)
        {
            GICO.WriteLine("OriginConnect: Closing Origin..");
            if (pOrigin != null)
            {
                pOrigin.Visible = MAINWND_VISIBLE.MAINWND_HIDE;
                pOrigin.Exit();


                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pOrigin);

                pWorksheet = null;
                pGraphLayer = null;
                pDataPlot = null;
                pOrigin = null;

                //int hWnd = pOrigin.Application.Hwnd;

                //Force KILL
                //uint processID; 
                //GetWindowThreadProcessId((IntPtr)hWnd, out processID);
                System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName("Origin~1");
                foreach (System.Diagnostics.Process p in procs)
                {
                    //if (p.Id == processID)
                    p.Kill();
                }
            }
        }
    }
}

