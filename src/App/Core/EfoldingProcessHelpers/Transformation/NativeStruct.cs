using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Core.eFolding.Transformation
{
    public class NativeStruct
    {
        public static List<int> DoGhostStructToNativeStruct(List<BasicStructs.Point> trajectoryToStruct, string nameFileGhostCube)
        {
            //Representa as posições do CUBO FANTASMA
            List<BasicStructs.Point> ghostCube = new List<BasicStructs.Point>();

            //Path do local de execução do executável
            string path = System.Environment.CurrentDirectory;
            ExtendedStreamReader ghostCubeFile = new ExtendedStreamReader(path + @"\" + nameFileGhostCube, new Guid(), false);
            //Carrega CUBO FANTASMA
            string line = string.Empty;
            while ((line = ghostCubeFile.ReadLine()) != null)
            {
                string[] data = line.Split('\t'); //separa informações através do caracter TAB
                data = data.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //limpa eventuais espaços em branco

                BasicStructs.Point temp = new BasicStructs.Point();
                temp.x = Convert.ToInt16(data[0]);
                temp.y = Convert.ToInt16(data[1]);
                temp.z = Convert.ToInt16(data[2]);
                ghostCube.Add(temp);
            }
            ghostCubeFile.Close();

            int cordX = 0;
            int cordY = 0;
            int cordZ = 0;
            List<int> newSequece = new List<int>();

            //Carrega ESTRUTURA SIMULADA
            foreach (BasicStructs.Point item in trajectoryToStruct)
            {
                cordX = Convert.ToInt16(item.x);
                cordY = Convert.ToInt16(item.y);
                cordZ = Convert.ToInt16(item.z);

                newSequece.Add(ghostCube.FindIndex(coord => coord.x == cordX && coord.y == cordY && coord.z == cordZ) + 1);
            }

            return newSequece;
        }

        public static List<int> DoPoc_GhostStructToNativeStruct()
        {
            Console.WriteLine("DoPoc_GhostStructToNativeStruct");
            //Representa as posições do CUBO FANTASMA
            List<BasicStructs.Point> ghostCube = new List<BasicStructs.Point>();

            //Path do local de execução do executável
            string path = System.Environment.CurrentDirectory;
            ExtendedStreamReader ghostCubeFile = new ExtendedStreamReader(path + @"\cuboFantasma.dat", new Guid(), false);
            //Carrega CUBO FANTASMA
            string line = string.Empty;
            while ((line = ghostCubeFile.ReadLine()) != null)
            {
                string[] data = line.Split('\t'); //separa informações através do caracter TAB
                data = data.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //limpa eventuais espaços em branco

                BasicStructs.Point temp = new BasicStructs.Point();
                temp.x = Convert.ToInt16(data[0]);
                temp.y = Convert.ToInt16(data[1]);
                temp.z = Convert.ToInt16(data[2]);
                ghostCube.Add(temp);
            }
            ghostCubeFile.Close();

            int cordX = 0;
            int cordY = 0;
            int cordZ = 0;
            List<int> newSequece = new List<int>();

            Console.WriteLine("De: ");
            //Motor de busca, ele retorna posição do INDICE da lista, em relação a condição de busca
            ExtendedStreamReader structOfSimulation = new ExtendedStreamReader(path + @"\00003.dat", new Guid(), false);
            //Carrega ESTRUTURA SIMULADA
            line = string.Empty;
            while ((line = structOfSimulation.ReadLine()) != null)
            {
                string[] data = line.Split('\t'); //separa informações através do caracter TAB
                data = data.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //limpa eventuais espaços em branco

                cordX = Convert.ToInt16(data[0]);
                cordY = Convert.ToInt16(data[1]);
                cordZ = Convert.ToInt16(data[2]);

                newSequece.Add(ghostCube.FindIndex(coord => coord.x == cordX && coord.y == cordY && coord.z == cordZ) + 1);

                Console.WriteLine("{0} {1} {2}", cordX, cordY, cordZ);

            }

            //Print a RESULTADO DA CONFIGURACAO
            Console.Write("Para: ");
            foreach (int item in newSequece)
            {
                Console.Write("{0} ", item);
            }
            Console.WriteLine();

            return newSequece;
        }
    }
}
