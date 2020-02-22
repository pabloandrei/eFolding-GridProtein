using System;
//using System.Linq;
using System.Collections.Generic;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
//using GridProteinFolding.CryptographyHelpers;
//using Microsoft.Win32;
//using GridProteinFolding.IOHelpers;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using System.ServiceModel;
using System.Net.NetworkInformation;
using GridProteinFolding.Middle.Helpers.RegistryHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Entities.Internal;
using GridProteinFolding.Entities.Membership;
using System.Linq;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;
using GridProteinFolding.Middle.Helpers.CryptographyHelpers;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using GridProteinFolding.Core.eFolding.MoteCarlo;
using GridProteinFolding.Core.eFolding.Structs;
using System.IO;
using GridProteinFolding.Core.eFolding.Transformation;
using System.Text.RegularExpressions;

[assembly: CLSCompliant(true)]
namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.Environment.CurrentDirectory;
            string fileSandBox = "TrajectorySandBox.dat";
            string fileGhostCube = "cuboFantasma.dat";

            for (int i = 1; i < System.IO.File.ReadLines(path + @"\" + fileSandBox).Count(); i++)
            {
                //efetua transformação do resultado da simulação (last trajectory por simulação), para padrao do cubo fantasma
                List<BasicStructs.Point> lastTrajectory = loadLastTRajectory(i, fileSandBox);
                List<int> newSequece = Transition.DoTransLastTrajectoryToNative(lastTrajectory, fileGhostCube);

                //cria expressão regular de busca
                string expression = string.Empty;
                foreach (int item in newSequece)
                {
                    expression = expression + String.Format("({0,3})", item);
                }
                Regex regex = new Regex(@expression);

                //motor de busca por arquivo
                var found = System.IO.File.ReadLines(path + @"\cfg51704.dat").Select((value, index) => new { index = index, value = value })
                   .Where(s => regex.IsMatch(s.value)).FirstOrDefault();

                if (found != null)
                {
                    Console.WriteLine("TrajectorySandBox id# {0}, cfg51704 id# {1} - [{2}]", i, found.index, found.value);
                }
            }

            Console.WriteLine("Done!");
        }

        private static List<BasicStructs.Point> loadLastTRajectory(int line, string nameFileOfTrajectories)
        {
            //Representa a estrutura do CUBO FANTASMA
            List<BasicStructs.Point> lastTrajectory = new List<BasicStructs.Point>();


            //Path do local de execução do executável
            string path = System.Environment.CurrentDirectory;
            string selectedLine = System.IO.File.ReadLines(path + @"\" + nameFileOfTrajectories).Skip(line - 1).Take(1).First();

            string[] data = selectedLine.Split('\t'); //separa informações através do caracter TAB
            data = data.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //limpa eventuais espaços em branco

            for (int i = 1; i < data.Count(); i = i + 3)
            {
                BasicStructs.Point temp = new BasicStructs.Point();
                temp.x = Convert.ToInt16(data[i]);
                temp.y = Convert.ToInt16(data[i + 1]);
                temp.z = Convert.ToInt16(data[i + 2]);
                lastTrajectory.Add(temp);
            }

            return lastTrajectory;
        }
    }
}







