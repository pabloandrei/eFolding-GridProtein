using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

[assembly: CLSCompliant(true)]
//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Middle.Helpers.ConfigurationHelpers
{
    public class Param
    {
        public DataToProcess dataToProcess;
        public DataToResults dataToResults;
        public Files files;
        public InternalProcess internalProcess;
        public List<Model> model;
        public ConfigApp configApp;
        public Output output;

        public static void PreperParams(ref GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param from, ref GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Param to)
        {

            to.configApp = new ConfigApp();
            to.configApp.MagicNumber = from.configApp.MagicNumber;
            to.configApp.EqualOne = from.configApp.EqualOne;

            to.dataToProcess = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.DataToProcess();
            to.dataToResults = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.DataToResults();
            to.files = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Files();
            to.internalProcess = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.InternalProcess();
            to.output = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Output();
            to.model = from.model;

            to.dataToProcess.Guid = from.dataToProcess.Guid;
            to.dataToProcess.isem = from.dataToProcess.isem;
            to.dataToProcess.maxInterations = from.dataToProcess.maxInterations;
            to.dataToProcess.valueOfDelta = from.dataToProcess.valueOfDelta;
            to.dataToProcess.totalSitio = from.dataToProcess.totalSitio;
            to.dataToProcess.loadDatFile = from.dataToProcess.loadDatFile;
            to.dataToProcess.file = from.dataToProcess.file;
            to.dataToProcess.maxMotionPeerIsem = from.dataToProcess.maxMotionPeerIsem;
            to.dataToProcess.model = from.dataToProcess.model;
            to.dataToProcess.modelType = from.dataToProcess.modelType;
            to.dataToProcess.beta = from.dataToProcess.beta;
            to.dataToProcess.temperature = from.dataToProcess.temperature;
            to.dataToProcess.crypt = from.dataToProcess.crypt;
            to.dataToProcess.recPathEvery = from.dataToProcess.recPathEvery;
            to.dataToProcess.splitFileEvery = from.dataToProcess.splitFileEvery;


            to.dataToResults.valueDiscard = from.dataToResults.valueDiscard;
            to.dataToResults.valueDivResult = from.dataToResults.valueDivResult;

            to.internalProcess.stopWhenSoft = true;

            to.files.Debug = from.files.Debug;

            if (from.dataToProcess.targets != null)
            {
                to.dataToProcess.targets = new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.Targets();
                to.dataToProcess.targets.targetsCoordinates = new List<GridProteinFolding.Middle.Helpers.ConfigurationHelpers.TargetsCoordinates>();

                to.dataToProcess.targets.id = from.dataToProcess.targets.id;
                to.dataToProcess.targets.description = from.dataToProcess.targets.description;

                foreach (GridProteinFolding.Middle.Helpers.ConfigurationHelpers.TargetsCoordinates targetCoord in from.dataToProcess.targets.targetsCoordinates)
                {
                    to.dataToProcess.targets.targetsCoordinates.Add(new GridProteinFolding.Middle.Helpers.ConfigurationHelpers.TargetsCoordinates() { id = targetCoord.id, targetsId = targetCoord.targetsId, value = targetCoord.value });
                }
            }

            to.output.configuration = from.output.configuration;
            //to.output.configurationJumpStep = from.output.configurationJumpStep;
            to.output.distribution = from.output.distribution;
            to.output.evolution = from.output.evolution;
            to.output.debug = from.output.debug;
            to.output.histogram = from.output.histogram;
            to.output.trajectory = from.output.trajectory;

        }

    }

    public class ConfigApp
    {
        double equalOne;
        int magicNumber;

        public double EqualOne
        {
            get
            {
                return equalOne;
            }

            set
            {
                equalOne = value;
            }
        }

        public int MagicNumber
        {
            get
            {
                return magicNumber;
            }

            set
            {
                magicNumber = value;
            }
        }
    }

    public class DataToProcess
    {
        private Guid guid;

        public Guid Guid
        {
            get { return guid; }
            set
            {
                guid = value;
            }
        }

        public Int32 totalSitio;
        public Int32 isem;
        public Int32 maxInterations;
        public Double valueOfDelta;
        public Boolean loadDatFile;
        public String file;
        public Int64 maxMotionPeerIsem;
        public Byte modelType;
        public String model;
        public double beta;
        public Targets targets;
        public double temperature;
        public bool crypt;
        public Int32 recPathEvery;
        public Int32 splitFileEvery;
    }

    public class DataToResults
    {
        public Int32 valueDivResult;
        public Int32 valueDiscard;
    }

    public class Files
    {
        private string debug;

        public string Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        public int count = 0;
    }

    public class InternalProcess
    {
        public bool stopWhenSoft = true;

    }

    public class Targets
    {
        public Int32 id;
        public String description;
        public List<TargetsCoordinates> targetsCoordinates;
    }

    public class TargetsCoordinates
    {
        public Int32 id;
        public Int32 targetsId;
        public Int32 value;
    }

    public class Model
    {
        public Int16 monomero;
        public Double value;
    }

    public class Output
    {
        public bool evolution;
        public bool distribution;
        public bool configuration;
        //public int configurationJumpStep;
        public bool debug;
        public bool histogram;
        public bool trajectory;
    }
}
