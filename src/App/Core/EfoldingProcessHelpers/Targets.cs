using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GridProteinFolding.Middle.Helpers.ConfigurationHelpers;
using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;
using static GridProteinFolding.Core.eFolding.Structs.BasicStructs;

namespace GridProteinFolding.Core.eFolding
{
    //public struct Cube
    //{
    //    public Point point;
    //    public int i; //indice da amostra do modelo
    //}

    //public struct Point
    //{
    //    public int x; //posição do ponto x
    //    public int y; //posição do ponto y
    //    public int z; //posição ponto z
    //}

    public class Targets
    {
        public static List<Point> coord = new List<Point>();

        public static List<Point> CreateTarget(string strucFull)
        {
            string[] struc = strucFull.Split(' '); //Quebra a linha em posições na array atravéz do espaço
            struc = struc.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //Remove os espaços em branco, deixa estrutura somente com posições válidas            

            if (coord.Count == 0)
            {
                //Lê o arquivo sample de geração das coordenadas
                string line;

                string path = System.Environment.CurrentDirectory;
                ExtendedStreamReader fileReader = new ExtendedStreamReader(path + @"\coord" + Directory.FileExtension, new Guid(), false);
                while ((line = fileReader.ReadLine()) != null)
                {
                    string[] tempFile = line.Split(' ');
                    tempFile = tempFile.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    coord.Add(new Point() { x = Convert.ToInt16(tempFile[0]), y = Convert.ToInt16(tempFile[1]), z = Convert.ToInt16(tempFile[2]) });
                }
                fileReader.Close();
            }

            //Incrimento inicial das posições do cubo x,y,z
            List<Point> points = new List<Point>(); //Uma lista encadeada do tipo PointCube
            for (int i = 0; i < coord.Count(); i++)
            {
                int pos = Convert.ToInt16(struc[i]) - 1;
                Point point = new Point() { x = coord[pos].x, y = coord[pos].y, z = coord[pos].z };
                points.Add(point);

                //Console.WriteLine("{0},{1},{2}", points[i].x, points[i].y, points[i].z);
            }

            return points;

        }

        public static List<Structs.BasicStructs.Point> CreateTarget(List<TargetsCoordinates> lstTargetsCoordinates)
        {

            //Efetua a leitura do modelo da "Sequência X"
            string strucFull = string.Empty; // "1  2  3  6  5  4  7  8  9 18 17 16 13 10 11 12 15 14 23 20 21 24 27 26 25 22 19";

            //string[] struc = strucFull.Split(' '); //Quebra a linha em posições na array atravéz do espaço
            //struc = struc.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //Remove os espaços em branco, deixa estrutura somente com posições válidas            

            string[] struc = new string[lstTargetsCoordinates.Count];
            int j = 0;
            foreach (TargetsCoordinates item in lstTargetsCoordinates)
            {
                struc[j++] = item.value.ToString();
            }


            //Lê o arquivo sample de geração das coordenadas
            string line;
            List<Structs.BasicStructs.Point> coord = new List<Structs.BasicStructs.Point>();

            ExtendedStreamReader file = new ExtendedStreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\coord" + Directory.FileExtension, Config.CurrentGuid, false);
            while ((line = file.ReadLine()) != null)
            {
                string[] temp = line.Split(' ');
                temp = temp.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                coord.Add(new Structs.BasicStructs.Point() { x = Convert.ToInt16(temp[0]), y = Convert.ToInt16(temp[1]), z = Convert.ToInt16(temp[2]) });
            }
            file.Close();


            //Incrimento inicial das posições do cubo x,y,z
            List<Point> points = new List<Point>(); //Uma lista encadeada do tipo PointCube
            for (int i = 0; i < coord.Count(); i++)
            {
                int pos = Convert.ToInt16(struc[i]) - 1;
                Point point = new Point() { x = coord[pos].x, y = coord[pos].y, z = coord[pos].z };
                points.Add(point);

                GICO.WriteLine(Config.CurrentGuid, String.Format("{0},{1},{2}", points[i].x, points[i].y, points[i].z));

            }

            //Cria um cadeia de simulação
            GCPS.chain = new Structs.BasicStructs.Chain();
            foreach (Point item in points)
            {
                GCPS.chain.r.Add(new Structs.BasicStructs.Point()
                {
                    x = item.x,
                    y = item.y,
                    z = item.z,
                    deadEnd = 0,
                    deadEndPoints = string.Empty,
                    neighbors = new Structs.BasicStructs.Neighbor[6],
                    e = new List<TypeE>()
                });

            }

            return GCPS.chain.r;
        }
    }
}


