using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;

namespace GridProteinFolding.Core.eFolding.MoveSet
{
    /// <summary>
    /// Esta Classe é responsável pelo movimento do tipo KINK
    /// </summary>
    public sealed class MoveKink : BaseMoves,  IDisposable
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
        //private static readonly MoveKink instance = new MoveKink();

        public MoveKink() { }

        //public static MoveKink Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}

        /// <summary>
        /// Destrutor Default
        /// </summary>
        ~MoveKink()
        {

        }

        /// <summary>
        /// Executa o Movimento de KINK, respeitando a regra para o mesmo.
        /// 1- Os Monomenos tem que estar no mesmo plano
        /// 2- Tem que percenter somente a um mesmo plano
        /// Para comprir, assim é definido pela distancia entre dois pontos.
        /// </summary>
        /// <returns></returns>
        public bool Do() {

            int x_ = GCPS.chain.r[GCPS.chain.selectNode - 1].x + GCPS.chain.r[GCPS.chain.selectNode + 1].x - GCPS.chain.r[GCPS.chain.selectNode].x;
            int y_ = GCPS.chain.r[GCPS.chain.selectNode - 1].y + GCPS.chain.r[GCPS.chain.selectNode + 1].y - GCPS.chain.r[GCPS.chain.selectNode].y;
            int z_ = GCPS.chain.r[GCPS.chain.selectNode - 1].z + GCPS.chain.r[GCPS.chain.selectNode + 1].z - GCPS.chain.r[GCPS.chain.selectNode].z;

            //copia dos dados do monomero para movimento
            Structs.BasicStructs.Point temp = new Structs.BasicStructs.Point() { x = x_, y = y_, z = z_ };
            temp.e = new List<TypeE>();
            temp.e.AddRange(GCPS.chain.r[GCPS.chain.selectNode].e);
             
            
            if (TryMove(ref temp))
            {              
                GCPS.tempChain.r[GCPS.chain.selectNode] = temp;
                GCPS.tempChain.contMoves.kinkAccept++;

                GCPS.tempChain.typeOfLattice = BasicEnums.Lattices.Kink;

                return true;
            }
            else
            {
                GCPS.chain.contMoves.kinkReject++;
                return false;
            }

        
        }      
    }
}
