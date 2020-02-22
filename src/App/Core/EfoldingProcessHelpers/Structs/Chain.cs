using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;

namespace GridProteinFolding.Core.eFolding.Structs
{
    /// <summary>
    /// Classe responsável pela estruturas os quais representam um MONOMERO e suas caracteristicas
    /// </summary>
    public partial class BasicStructs
    {
        /// <summary>
        /// Representa estrutura de um MONOMERO
        /// </summary>
        public class Chain : Base
        {
            /// <summary>
            /// Construtor publico do MONOMERO
            /// </summary>
            public Chain()
            {
                r = new List<BasicStructs.Point>();
                isem = GCPS.initialIsem;
                UPrevious = 0;
            }

            /// <summary>
            /// Efetua copia temporário de um monomero
            /// </summary>
            public void CopyToTempMonomero()
            {

                GCPS.tempChain.r.Clear();

                GCPS.tempChain.isem = GCPS.chain.isem;

                //Copia lista de posicoes da cadeia
                GCPS.tempChain.r = new List<BasicStructs.Point>(GCPS.chain.r.Count);

                for (int i = 0; i < this.r.Count; i++)
                {
                    GCPS.tempChain.r.Add(
                        new Point()
                        {
                            x = GCPS.chain.r[i].x,
                            y = GCPS.chain.r[i].y,
                            z = GCPS.chain.r[i].z,
                            e = new List<TypeE>(),
                            classificationMotion = GCPS.chain.r[i].classificationMotion,
                            deadEnd = GCPS.chain.r[i].deadEnd,
                            deadEndPoints = GCPS.chain.r[i].deadEndPoints,
                            neighbors = GCPS.chain.r[i].neighbors
                        });

                    for (int j = 0; j < GCPS.chain.r[i].e.Count; j++)
                    {

                        GCPS.tempChain.r[i].e.Add(GCPS.chain.r[i].e[j]);
                    }
                };

                GCPS.tempChain.typeOfLattice = this.typeOfLattice;
                GCPS.tempChain.contMoves = this.contMoves;
                GCPS.tempChain.selectNode = this.selectNode;

            }

            /// <summary>
            /// Aceita um movimento válido
            /// </summary>
            public void AcceptMotion()
            {
                //Antes copia U atual
                GCPS.chain.UPrevious = GCPS.chain.U;
                GCPS.chain.typeOfLattice = GCPS.tempChain.typeOfLattice;
                GCPS.chain.contMoves = GCPS.tempChain.contMoves;

                //Copia lista de posicoes da cadeia
                this.r = new List<BasicStructs.Point>(GCPS.tempChain.r.Count);
                GCPS.tempChain.r.ForEach((item) =>
                {
                    this.r.Add(
                        new Point()
                        {
                            x = item.x,
                            y = item.y,
                            z = item.z,
                            e = item.e,
                            classificationMotion = item.classificationMotion,
                            deadEnd = item.deadEnd,
                            deadEndPoints = item.deadEndPoints,
                            neighbors = item.neighbors
                        });
                });
            }
        }
    }
}
