using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SandBox
{
    class TestGeradorDoCubo
    {
        ////    #region Teste gerador CUBO
        ////    class Program
        ////    {
        ////        struct Cube
        ////        {
        ////            public Point point;
        ////            public int i; //indice da amostra do modelo
        ////        }

        ////        struct Point
        ////        {
        ////            public int x; //posição do ponto x
        ////            public int y; //posição do ponto y
        ////            public int z; //posição ponto z
        ////        }

        ////        //static void Main(string[] args)
        ////        //{
        ////        //    //Efetua a leitura do modelo da "Sequência X"
        ////        //    string strucFull = "1  2  3  6  5  4  7  8  9 18 17 16 13 10 11 12 15 14 23 20 21 24 27 26 25 22 19";
        ////        //    string[] struc = strucFull.Split(' '); //Quebra a linha em posições na array atravéz do espaço
        ////        //    struc = struc.Where(x => !string.IsNullOrEmpty(x)).ToArray(); //Remove os espaços em branco, deixa estrutura somente com posições válidas            


        ////        //    //Lê o arquivo sample de geração das coordenadas
        ////        //    string line;
        ////        //    List<Point> coord = new List<Point>();
        ////        //    ExtendedStreamReader fileReader = new ExtendedStreamReader(@"C:\Users\silvapab\Source\Workspaces_CodePlex\GridProteinFolding\eFolding\SandBox\SandBox\coord" + Directory.FileExtension, new Guid(), false);
        ////        //    while ((line = fileReader.ReadLine()) != null)
        ////        //    {
        ////        //        string[] tempFile = line.Split(' ');
        ////        //        tempFile = tempFile.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        ////        //        coord.Add(new Point() { x = Convert.ToInt16(tempFile[0]), y = Convert.ToInt16(tempFile[1]), z = Convert.ToInt16(tempFile[2]) });
        ////        //    }
        ////        //    fileReader.Close();


        ////        //    //Incrimento inicial das posições do cubo x,y,z
        ////        //    List<Cube> cube = new List<Cube>(); //Uma lista encadeada do tipo PointCube
        ////        //    for (int i = 0; i < coord.Count(); i++)
        ////        //    {
        ////        //        int pos = Convert.ToInt16(struc[i]) - 1;
        ////        //        Point point = new Point() { x = coord[pos].x, y = coord[pos].y, z = coord[pos].z };
        ////        //        cube.Add(new Cube() { i = pos, point = point });

        ////        //        Console.WriteLine("{0},{1},{2}", cube[i].point.x, cube[i].point.y, cube[i].point.z);

        ////        //    }


        ////        //    //Cria um cadeia de simulação
        ////        //    GCPS.chain = new GridProteinFolding.Core.eFolding.Structs.BasicStructs.Chain();
        ////        //    foreach (Cube guid in cube)
        ////        //    {
        ////        //        GCPS.chain.r.Add(new GridProteinFolding.Core.eFolding.Structs.BasicStructs.Point()
        ////        //        {
        ////        //            x = guid.point.x,
        ////        //            y = guid.point.y,
        ////        //            z = guid.point.z,
        ////        //            deadEnd = 0,
        ////        //            deadEndPoints = string.Empty,
        ////        //            neighbors = new GridProteinFolding.Core.eFolding.Structs.BasicStructs.Neighbor[6],
        ////        //            e = new List<GridProteinFolding.Core.eFolding.Structs.TypeE>()
        ////        //        });


    }
}
