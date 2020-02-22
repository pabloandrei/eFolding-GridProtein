namespace GridProteinFolding.ETL.HostETL
{
    class Program
    {
        static void Main(string[] args)
        {
            ResultProcess objResultProcess = new ResultProcess();
            objResultProcess.Process();
            objResultProcess = null;
        }
    }
}
