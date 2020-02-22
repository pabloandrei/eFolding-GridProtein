using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using Config = GridProteinFolding.Middle.Helpers.ConfigurationHelpers.AppConfigClient;

namespace GridProteinFolding.Core.eFolding.Structs
{
    public partial class BasicStructs
    {
        /// <summary>
        /// Representa estrutura de um TempMONOMERO
        /// </summary>
        public class TemporaryChain : Base
        {
            public TemporaryChain()
            {
                r = new List<BasicStructs.Point>();
            }


            #region Neighbors

            /// <summary>
            /// Gera classificação geral do resíduo
            /// -A cadeia tem N resíduos
            ///-A coordenada do resíduo “i” da cadeia: i = 1, 2, 3, ...   27.  é escrita como 
            ///  x(i), y(i), z(i).  Pode também ser uma matriz (27 3): R(i, j), com j=1,2,3 
            ///                         R(i, 1) =  x(i);   R(i, 2) = y(i);   R(i, 3) = z(i).
            ///-Os vizinhos do resíduo "i”: são descritos numa TABELA, descrevendo os seus seis possíveis vizinhos, isto é, uma Matriz M(N,6)
            ///    M(i,1) = status do sítio x(i)+1, y(i),  z(i)
            ///    M(i,2) = status do sítio x(i) -1, y(i),  z(i)
            ///    M(i,3) = status do sítio x(i),  y(i)+1, z(i)
            ///    M(i,4) = status do sítio x(i),  y(i) -1, z(i)
            ///    M(i,5) = status do sítio x(i), y(i),  z(i)+1
            ///    M(i,6) = status do sítio x(i), y(i),  z(i) -1
            ///        O status de um sítio pode ser:
            ///                        -ocupado por um resíduo vizinho da cadeia:  M(i,j) = -1; 
            ///                        -ocupado por um vizinho topológico           :  M(i,j) = +1;
            ///                        -sítio vazio                                 :  M(i,j) = 0.
            ///A marcação dos vizinhos deve ser feita para todos os resíduos da cadeia.        
            /// </summary>            
            public enum NeighborsType { neighborChain = -1, neighborTopological = 1, emptySite = 0 };


            public void NeighborsUpdateAll()
            {
                for (int i = 0; i < GCPS.tempChain.r.Count; i++)
                {
                    CalculatingEnergy.DelEnergyFWD(i);
                }

                List<BasicStructs.Point> points = this.r;
                for (int i = 0; i < points.Count; i++)
                {
                    BasicStructs.Point temp = points[i];
                    temp.neighbors = NeighborsProcess(points[i], i, true);
                    points[i] = temp;
                }

                this.r = points;
            }

            //public void NeighborsUpdatePeerPoint(int i)
            //{
            //    List<Structs.Point> points = this.r;
            //    Structs.Point temp = points[i];
            //    temp.neighbors = NeighborsProcess(points[i], i, true);
            //    points[i] = temp;

            //    this.r = points;
            //}


            internal static BasicStructs.Neighbor[] NeighborsProcess(BasicStructs.Point point, int selectNode, bool calcEnergy)
            {
                BasicStructs.Point originalPoint = point;

                point.x++;
                BasicStructs.Neighbor M1 = NeighborsCheck(point, selectNode, 0, calcEnergy);

                point = originalPoint;
                point.x--;
                BasicStructs.Neighbor M2 = NeighborsCheck(point, selectNode, 1, calcEnergy);

                point = originalPoint;
                point.y++;
                BasicStructs.Neighbor M3 = NeighborsCheck(point, selectNode, 2, calcEnergy);

                point = originalPoint;
                point.y--;
                BasicStructs.Neighbor M4 = NeighborsCheck(point, selectNode, 3, calcEnergy);

                point = originalPoint;
                point.z++;
                BasicStructs.Neighbor M5 = NeighborsCheck(point, selectNode, 4, calcEnergy);

                point = originalPoint;
                point.z--;
                BasicStructs.Neighbor M6 = NeighborsCheck(point, selectNode, 5, calcEnergy);

                return new BasicStructs.Neighbor[6] { M1, M2, M3, M4, M5, M6 };
            }

            internal static BasicStructs.Neighbor NeighborsCheck(BasicStructs.Point tempCoord, int selectNode, int M, bool calcEnergy)
            {
                for (int i = 0; i < GridProteinFolding.Core.eFolding.Simulation.tempChain.r.Count; i++)
                {
                    BasicStructs.Point temp = GCPS.tempChain.r[i];

                    //ocupado por um resíduo vizinho da cadeia: M(i,j) = -1; 
                    if (temp.x == tempCoord.x &&
                        temp.y == tempCoord.y &&
                        temp.z == tempCoord.z && (((selectNode + 1) == i) || ((selectNode - 1) == i)))
                    {

                        return new BasicStructs.Neighbor()
                        {
                            classification = NeighborsType.neighborChain,
                            contacResidue = i
                        };
                    }
                    //ocupado por um vizinho topológico :  M(i,j) = +1;
                    else if (temp.x == tempCoord.x &&
                        temp.y == tempCoord.y &&
                        temp.z == tempCoord.z)
                    {

                        if (calcEnergy)
                        {
                            CalculatingEnergy.AddEnergy(GCPS.energeticMatrix[selectNode, i], selectNode, i);
                        }

                        return new BasicStructs.Neighbor()
                        {
                            classification = NeighborsType.neighborTopological,
                            contacResidue = i
                        };
                    }

                }
                //sítio vazio:  M(i,j) = 0.

                return new BasicStructs.Neighbor()
                {
                    classification = NeighborsType.emptySite,
                    contacResidue = -1
                };
            }

            /// <summary>
            /// Salva em arquivo o contato topológico de vizinhos de primeiro contato
            /// </summary>
            /// <param name="isem"></param>
            public void NeighborsSave(long isem, ref int numberNeighborTopological)
            {
                string file = isem.ToString();
                //Members.initialIsem = Members.isem

                string dir = IO.Directory.SubDirNeighbors;
                string pathFile = dir + @"\" + file + Consts.extensionOfFile;

                ExtendedStreamWriter sw = new ExtendedStreamWriter(Config.Crypt);
                sw.CreateText(pathFile, Config.CurrentGuid);

                int numberContact = 0;
                numberNeighborTopological = 0;
                //sw.WriteLine("Residuo | 1o.Vz.| 2o.Vz.| 3o.Vz.| 4o.Vz.| 5o.Vz.| 6o.Vz.|");

                List<BasicStructs.Point> points = this.r;
                //foreach (Structs.Point temp in points)
                for (int i = 0; i < points.Count; i++)
                {
                    BasicStructs.Point temp = points[i];
                    sw.Write("{0}\t", i);

                    if (temp.neighbors != null)
                    {
                        bool ocuredContact = false;
                        for (int j = 0; j < temp.neighbors.Length; j++)
                        {
                            if (temp.neighbors[j].classification == NeighborsType.neighborTopological)
                            {
                                numberContact++;
                                ocuredContact = true;
                                sw.Write("{0}\t", temp.neighbors[j].classification.ToString() + "(" + temp.neighbors[j].contacResidue.ToString() + ")");
                            }
                        }
                        if (ocuredContact)
                            numberNeighborTopological++;
                    }
                    sw.WriteLine();
                }

                sw.WriteLine();
                sw.WriteLine("numberContact:" + numberContact);
                sw.WriteLine("numberNeighborTopological:" + numberNeighborTopological);
                //this.numberNeighborTopological = numberNeighborTopological;
                sw.Flush();
                sw.Close();
            }


            #endregion
        }
    }
}
