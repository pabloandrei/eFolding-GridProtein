using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Middle.Helpers.RandomHelpers;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;

namespace GridProteinFolding.Core.eFolding.MoveSet
{
    /// <summary>
    /// Esta Classe é responsável pelo movimento do tipo ENDS
    /// </summary>
    public sealed class MoveEnds : BaseMoves,  IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/ff650316.aspx
        /// </summary>
        //private static readonly MoveEnds instance = new MoveEnds();

        public MoveEnds() { }

        //public static MoveEnds Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}

        /// <summary>
        /// Destrutor Default
        /// </summary>
        ~MoveEnds()
        {

        }

        //Sorteiro de possibilidade
        //int sortPossible = int.MinValue;

        enum end { none = 0, one = 1, two = 2, three = 3, four = 4 };

        /// <summary>
        /// Este Metodo executa o movimento de End.
        /// </summary>
        /// <returns>Return TRUE se o movimento ocorreu</returns>
        public bool Do() {

            Structs.BasicStructs.Point r1;
            Structs.BasicStructs.Point r2;

            if (GCPS.chain.selectNode == 0)
            {
                //r1 = Members.monomero.r[Members.monomero.selectNode];
                //r2 = Members.monomero.r[Members.monomero.selectNode + 1];

                //copia dos dados do monomero para movimento
                r1 = new Structs.BasicStructs.Point()
                {
                    x = GCPS.chain.r[GCPS.chain.selectNode].x,
                    y = GCPS.chain.r[GCPS.chain.selectNode].y,
                    z = GCPS.chain.r[GCPS.chain.selectNode].z };
                r1.e = new List<TypeE>();
                r1.e.AddRange(GCPS.chain.r[GCPS.chain.selectNode].e);

                //copia dos dados do monomero para movimento
                r2 = new Structs.BasicStructs.Point()
                {
                    x = GCPS.chain.r[GCPS.chain.selectNode + 1].x,
                    y = GCPS.chain.r[GCPS.chain.selectNode + 1].y,
                    z = GCPS.chain.r[GCPS.chain.selectNode + 1].z
                };
                r2.e = new List<TypeE>();
                r2.e.AddRange(GCPS.chain.r[GCPS.chain.selectNode + 1].e);
            }
            else
            {
                //r1 = Members.monomero.r[Members.monomero.selectNode];
                //r2 = Members.monomero.r[Members.monomero.selectNode - 1];
                
                //copia dos dados do monomero para movimento
                r1 = new Structs.BasicStructs.Point()
                {
                    x = GCPS.chain.r[GCPS.chain.selectNode].x,
                    y = GCPS.chain.r[GCPS.chain.selectNode].y,
                    z = GCPS.chain.r[GCPS.chain.selectNode].z
                };
                r1.e = new List<TypeE>();
                r1.e.AddRange(GCPS.chain.r[GCPS.chain.selectNode].e);

                //copia dos dados do monomero para movimento
                r2 = new Structs.BasicStructs.Point()
                {
                    x = GCPS.chain.r[GCPS.chain.selectNode - 1].x,
                    y = GCPS.chain.r[GCPS.chain.selectNode - 1].y,
                    z = GCPS.chain.r[GCPS.chain.selectNode - 1].z
                };
                r2.e = new List<TypeE>();
                r2.e.AddRange(GCPS.chain.r[GCPS.chain.selectNode - 1].e);
            }

            //Vetor p, como sendo a difereça entre os vetores posuicao do residuo "2" e do residuo "1", isto é: p=r1-r1
            Structs.BasicStructs.Point p = new Structs.BasicStructs.Point()
            {
                x = r2.x - r1.x,
                y = r2.y - r1.y,
                z = r2.z - r1.z
            };


            //Definicao dos dois vetores v e w
            Structs.BasicStructs.Point v;
            Structs.BasicStructs.Point w;
            //cria as 3 possibilidades para v e w
            if (p.x != 0)
            {
                v = new Structs.BasicStructs.Point() { x = 0, y = p.x, z = 0 };
                w = new Structs.BasicStructs.Point() { x = 0, y = 0, z = p.x };
            }
            else if (p.y != 0)
            {
                v = new Structs.BasicStructs.Point() { x = 0, y = 0, z = p.y };
                w = new Structs.BasicStructs.Point() { x = p.y, y = 0, z = 0 };
            }
            else //if (p.z != 0)
            {
                v = new Structs.BasicStructs.Point() { x = p.z, y = 0, z = 0 };
                w = new Structs.BasicStructs.Point() { x = 0, y = p.z, z = 0 };
            }


            //Sorteio do movimento para 4 possibilidades (de 0 à 3)
            Randomic.Random();
            int i = (int)(Randomic.randu * 4);

            end type = end.none;

            switch (i)
            {
                case 0:
                    //Primeira possibilidade: r1=r2+vi
                    r1.x = r2.x + v.x;
                    r1.y = r2.y + v.y;
                    r1.z = r2.z + v.z;
                    type = end.one;
                    break;

                case 1:
                    //Segunda possibilidade: r1=r2-vi
                    r1.x = r2.x - v.x;
                    r1.y = r2.y - v.y;
                    r1.z = r2.z - v.z;
                    type = end.two;
                    break;

                case 2:
                    //Terceira possibilidade: r1=r2+wi
                    r1.x = r2.x + w.x;
                    r1.y = r2.y + w.y;
                    r1.z = r2.z + w.z;
                    type = end.three;
                    break;

                case 3:
                    //Quarta possibilidade: r1=r2-wi
                    r1.x = r2.x - w.x;
                    r1.y = r2.y - w.y;
                    r1.z = r2.z - w.z;
                    type = end.four;
                    break;
                default:                    
                    new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().Exception(new System.Exception("Error ends.."), Types.ErrorLevel.Warning);
                    break;
            }

            //GICO.WriteLine("{0} {1} {2}", r1.x, r1.y, r1.z);

            //Verifica a aceitação do movimento
            if (TryMove(ref r1)) // && TryMovePeerDPP(ref pointSorted, (Structs.Point)Members.monomero.r[neighborNode]))
            {
                GCPS.tempChain.r[GCPS.chain.selectNode] = r1;
                GCPS.tempChain.contMoves.endsAccept++;

                switch (type)
                {
                    case end.one:
                        GCPS.tempChain.contMoves.moveSetEnd.endAccept_1++;
                        break;
                    case end.two:
                        GCPS.tempChain.contMoves.moveSetEnd.endAccept_2++;
                        break;
                    case end.three:
                        GCPS.tempChain.contMoves.moveSetEnd.endAccept_3++;
                        break;
                    case end.four:
                        GCPS.tempChain.contMoves.moveSetEnd.endAccept_4++;
                        break;
                }

                GCPS.tempChain.typeOfLattice = BasicEnums.Lattices.End;

                return true;
            }
            else
            {
                GCPS.chain.contMoves.endsReject++;

                switch (type)
                {
                    case end.one:
                        GCPS.chain.contMoves.moveSetEnd.endReject_1++;
                        break;
                    case end.two:
                        GCPS.chain.contMoves.moveSetEnd.endReject_2++;
                        break;
                    case end.three:
                        GCPS.chain.contMoves.moveSetEnd.endReject_3++;
                        break;
                    case end.four:
                        GCPS.chain.contMoves.moveSetEnd.endReject_4++;
                        break;
                }
                return false;
            }
        }        
    }
}
