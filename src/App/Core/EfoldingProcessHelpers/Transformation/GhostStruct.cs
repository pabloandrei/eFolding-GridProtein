using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Core.eFolding.Transformation
{
    public class GhostStruct
    {
        public static List<BasicStructs.Point> DoNativeStructToGhostStruct(List<int> nativeStruct)
        {
            //Representa a estrutura do CUBO FANTASMA
            List<BasicStructs.Point> ghostCube = new List<BasicStructs.Point>();
            //Representa qual linha foi escolhida, no arquivo de configuração
            int[] seqToMakeNewStruct = new int[27];
            //Representa a nova estrutura a ser gerada
            List<BasicStructs.Point> newStruct = new List<BasicStructs.Point>();

            //Path do local de execução do executável
            string path = System.Environment.CurrentDirectory;
            ExtendedStreamReader ghostCubeFile = new ExtendedStreamReader(path + @"\cuboFantasma.dat", new Guid(), false);


            //Leitura das COORDENADAS BASICAS para os 27 monomeros(coordenadas estabelecidas arbitrariamente, mas fixas)            
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

            //Gera mapeamento da nova estrutura
            int ik = 0;
            foreach (int item in nativeStruct)
            {
                seqToMakeNewStruct[ik] = item;
                ik++;
            }

            //Guardar as coordenadas para cada monomero da ie configuracao desejada
            for (int ipos = 0; ipos < 27; ipos++)
            {
                BasicStructs.Point temp = new BasicStructs.Point();

                int imon = seqToMakeNewStruct[ipos] - 1;
                temp.x = ghostCube[imon].x;
                temp.y = ghostCube[imon].y;
                temp.z = ghostCube[imon].z;

                newStruct.Add(temp);
            }

            return newStruct;
        }

        public static List<BasicStructs.Point> DoPoc_NativeStructToGhostStruct()
        {
            Console.WriteLine("DoPoc_NativeStructToGhostStruct");
            //Representa a estrutura do CUBO FANTASMA
            List<BasicStructs.Point> ghostCube = new List<BasicStructs.Point>();
            //Representa qual linha foi escolhida, no arquivo de configuração
            int[] seqToMakeNewStruct = new int[27];
            //Representa a nova estrutura a ser gerada
            List<BasicStructs.Point> newStruct = new List<BasicStructs.Point>();

            // Observações:
            //cfg51704.dat :  arquivo da sequencia posicional dos 27 monomeros
            //cuboFantasma.dat    :  coordenadas(x, y, z) dos 27 vértices do cubo 3X3X3

            //Path do local de execução do executável
            string path = System.Environment.CurrentDirectory;
            ExtendedStreamReader ghostCubeFile = new ExtendedStreamReader(path + @"\cuboFantasma.dat", new Guid(), false);


            //Leitura das COORDENADAS BASICAS para os 27 monomeros(coordenadas estabelecidas arbitrariamente, mas fixas)            
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

            //ID da estrutura a ser gerada
            int idOfConfig = 3; //equivale arquivo 00003.dat

            //Le a linha da estrutura escolhida
            string[] selectedLine = new string[27];
            selectedLine = System.IO.File.ReadLines(path + @"\cfg51704.dat").Skip(idOfConfig - 1).Take(1).First().Split(' '); //Lê linha escolhida diretamente do arquivo
            selectedLine = selectedLine.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //limpa eventuais espaços em branco


            Console.Write("De: ");
            for (int i = 0; i < selectedLine.Count(); i++)
            {
                Console.Write("{0} ", selectedLine[i]);
            }
            Console.WriteLine();

            //Gera mapeamento da nova estrutura
            int ik = 0;
            foreach (string item in selectedLine)
            {
                seqToMakeNewStruct[ik] = Convert.ToInt16(item);
                ik++;
            }

            //Guardar as coordenadas para cada monomero da ie configuracao desejada
            for (int ipos = 0; ipos < 27; ipos++)
            {
                BasicStructs.Point temp = new BasicStructs.Point();

                int imon = seqToMakeNewStruct[ipos] - 1;
                temp.x = ghostCube[imon].x;
                temp.y = ghostCube[imon].y;
                temp.z = ghostCube[imon].z;

                newStruct.Add(temp);
            }

            Console.WriteLine("Para: ");
            //Print a nova ESTRUTURA
            for (int ipos = 0; ipos < 27; ipos++)
            {
                Console.WriteLine("{0} {1} {2}", newStruct[ipos].x, newStruct[ipos].y, newStruct[ipos].z);
            }

            return newStruct;
        }
    }
}












