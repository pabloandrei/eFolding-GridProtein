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
    /// Esta Classe é responsável pelo movimento do tipo Crankshaft
    /// Uma manivela exite no plano, ao longo da direção X, Y ou Z:
    /// 1-As coordenadas de quatro resíduos consecutivos, Ri; Ri+1; Ri+2; Ri+3 de uma configuração válida (cadeia real), definem uma manivela planar se a distância Di,i+3 ao quadrado entre o Ri e Ri+3 for igual a 1, isto é (Di,i+3)2 = 1 
    /// 2- Assim, manivelas na direção X, Y ou Z, são definidas em função do primeiro resíduo (o da esquerda)
    /// </summary>
    public sealed class MoveCrankshaft : BaseMoves, IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        public MoveCrankshaft() { }

        ~MoveCrankshaft() { }

        enum node { unknown, isR0, isR1, isR2, isR3 };

        enum crankshaft { none = 0, one = 1, two = 2, three = 3 };

        /// <summary>
        /// Executa teste de validação de MONOMERO seleciona é um crankShaft
        /// </summary>
        /// <returns></returns>
        public bool Do()
        {
            int internalSelectNode = GCPS.chain.selectNode;

            //Movimento de crankShaft é sempre em função de R1
            //Abaixo efetuaremos o ajustes do Ri
            if (GCPS.chain.r[internalSelectNode].classificationMotion.crankShaft__R0)
                internalSelectNode++;
            else if (GCPS.chain.r[internalSelectNode].classificationMotion.crankShaft__R2)
                internalSelectNode--;
            else if (GCPS.chain.r[internalSelectNode].classificationMotion.crankShaft__R3)
                internalSelectNode = internalSelectNode - 2;

            // |P = |v3 - |v0
            int Px = GCPS.chain.r[internalSelectNode + 1].x - GCPS.chain.r[internalSelectNode - 1].x;
            int Py = GCPS.chain.r[internalSelectNode + 1].y - GCPS.chain.r[internalSelectNode - 1].y;
            int Pz = GCPS.chain.r[internalSelectNode + 1].z - GCPS.chain.r[internalSelectNode - 1].z;

            // |Q = |v1 - |v0
            int Qx = GCPS.chain.r[internalSelectNode].x - GCPS.chain.r[internalSelectNode - 1].x;
            int Qy = GCPS.chain.r[internalSelectNode].y - GCPS.chain.r[internalSelectNode - 1].y;
            int Qz = GCPS.chain.r[internalSelectNode].z - GCPS.chain.r[internalSelectNode - 1].z;

            //O vetor |V é perpendicular a |P e a |Q simultaneamente
            int Vx = (Py * Qz) - (Qy * Pz);  //em direção de X
            int Vy = (Pz * Qx) - (Qz * Px);  //em direção de Y
            int Vz = (Px * Qy) - (Qx * Py);  //em direção de Z

            //Tres possibilidades de movimento (3)
            int R_x = 0;
            int R_y = 0;
            int R_z = 0;
            int R_x_1 = 0;
            int R_y_1 = 0;
            int R_z_1 = 0;

            //Sorteio do movimento
            Randomic.Random();
            int sort = (int)(Randomic.randu * 3);

            crankshaft type = crankshaft.none;
            switch (sort)
            {
                case 0:
                    //Primeira possibilidade (1)
                    //onde |r´i = |ri-1 + |V
                    R_x = GCPS.chain.r[internalSelectNode - 1].x + Vx;
                    R_y = GCPS.chain.r[internalSelectNode - 1].y + Vy;
                    R_z = GCPS.chain.r[internalSelectNode - 1].z + Vz;
                    //onde |r´i+1 = |ri+2 + |V
                    R_x_1 = GCPS.chain.r[internalSelectNode + 2].x + Vx;
                    R_y_1 = GCPS.chain.r[internalSelectNode + 2].y + Vy;
                    R_z_1 = GCPS.chain.r[internalSelectNode + 2].z + Vz;

                    type = crankshaft.one;
                    break;

                case 1:
                    //Segunda possibilidade (2)
                    //onde |r´i = |ri-1 - |V
                    R_x = GCPS.chain.r[internalSelectNode - 1].x - Vx;
                    R_y = GCPS.chain.r[internalSelectNode - 1].y - Vy;
                    R_z = GCPS.chain.r[internalSelectNode - 1].z - Vz;
                    //onde |r´i+1 = |ri+2 - |V
                    R_x_1 = GCPS.chain.r[internalSelectNode + 2].x - Vx;
                    R_y_1 = GCPS.chain.r[internalSelectNode + 2].y - Vy;
                    R_z_1 = GCPS.chain.r[internalSelectNode + 2].z - Vz;

                    type = crankshaft.two;
                    break;

                case 2:
                    //Terceira possibilidade (3)
                    //onde |r´i = |ri-1 - |Q
                    R_x = GCPS.chain.r[internalSelectNode - 1].x - Qx;
                    R_y = GCPS.chain.r[internalSelectNode - 1].y - Qy;
                    R_z = GCPS.chain.r[internalSelectNode - 1].z - Qz;
                    //onde |r´i+1 = |ri+2 - |Q
                    R_x_1 = GCPS.chain.r[internalSelectNode + 2].x - Qx;
                    R_y_1 = GCPS.chain.r[internalSelectNode + 2].y - Qy;
                    R_z_1 = GCPS.chain.r[internalSelectNode + 2].z - Qz;

                    type = crankshaft.three;
                    break;
                default:
                    new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().Exception(new System.Exception("Error Sort"), Types.ErrorLevel.Warning);
                    break;
            }

            Structs.BasicStructs.Point temp01 = new Structs.BasicStructs.Point() { x = R_x, y = R_y, z = R_z };
            temp01.e = new List<TypeE>();
            temp01.e.AddRange(GCPS.chain.r[internalSelectNode].e);


            Structs.BasicStructs.Point temp02 = new Structs.BasicStructs.Point() { x = R_x_1, y = R_y_1, z = R_z_1 };
            temp02.e = new List<TypeE>();
            temp02.e.AddRange(GCPS.chain.r[internalSelectNode + 1].e);


            if (TryMove(ref temp01) && TryMove(ref temp02))
            {
                GCPS.tempChain.r[internalSelectNode] = temp01;
                GCPS.tempChain.r[internalSelectNode + 1] = temp02;
                GCPS.tempChain.contMoves.crankshaftAccept++;

                switch (type)
                {
                    case crankshaft.one:
                        GCPS.tempChain.contMoves.moveSetCrankshaft.crankshaftAccept_1++;
                        break;
                    case crankshaft.two:
                        GCPS.tempChain.contMoves.moveSetCrankshaft.crankshaftAccept_2++;
                        break;
                    case crankshaft.three:
                        GCPS.tempChain.contMoves.moveSetCrankshaft.crankshaftAccept_3++;
                        break;
                }

                GCPS.tempChain.typeOfLattice = BasicEnums.Lattices.Crank;

                return true;
            }
            else
            {
                GCPS.chain.contMoves.crankshaftReject++;

                switch (type)
                {
                    case crankshaft.one:
                        GCPS.chain.contMoves.moveSetCrankshaft.crankshaftReject_1++;
                        break;
                    case crankshaft.two:
                        GCPS.chain.contMoves.moveSetCrankshaft.crankshaftReject_2++;
                        break;
                    case crankshaft.three:
                        GCPS.chain.contMoves.moveSetCrankshaft.crankshaftReject_3++;
                        break;
                }

                return false;
            }
        }
    }
}