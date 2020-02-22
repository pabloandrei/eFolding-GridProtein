using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Middle.Helpers.EnumsHelpers;


namespace GridProteinFolding.Core.eFolding.Structs
{
    /// <summary>
    /// Classe Base
    /// </summary>
    public class Base : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        #region Attributs
        /// <summary>
        /// Isem o qual gerou o Monomero
        /// </summary>
        public long isem;

        /// <summary>
        /// Energia inicial da configuração do monomero de posição ZERO
        /// </summary>
        public double UZeroPosition = 0;

        /// <summary>
        /// Representação da "Fila da Cadeia"
        /// Ela esta protecteda e estatica para ser compartilhada por todas as rotinas e metodos do projeto
        /// </summary>
        public List<BasicStructs.Point> r = new List<BasicStructs.Point>();

        /// <summary>
        /// Tipo da cadeia
        /// </summary>
        public BasicEnums.ChainTypes? chainTypes;

        /// <summary>
        /// Posição do residuo para a tentativa de movimento
        /// </summary>
        public int selectNode = -1;

        ///// <summary>
        ///// Valor atual de delatU
        ///// </summary>
        //public double deltaU = 0;

        /// <summary>
        /// Valor anteior de U
        /// </summary>
        public double UPrevious = 0;

        /// <summary>
        /// Valores pertinentes a movimentos na cadeia
        /// </summary>
        public BasicStructs.MoveSetApplies contMoves;

        public int numberNeighborTopological = 0;

        /// <summary>
        /// Contém tipo de Lattice da semente em questão
        /// </summary>
        public GridProteinFolding.Middle.Helpers.EnumsHelpers.BasicEnums.Lattices typeOfLattice = BasicEnums.Lattices.None;

        /// <summary>
        /// Valor pertinente a energia de interação
        /// </summary>
        public string interationEnergy = string.Empty;

        #endregion

        #region Fields
        /// <summary>
        /// Método de Clone
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Total de contatos topologicos do MONOMERO
        /// </summary>
        /// <returns></returns>
        public double TotalNeighborTopological()
        {

            double ret = 0;
            for (int i = 0; i < this.r.Count; i++)
            {
                if (this.r[i].neighbors != null)
                {
                    for (int j = 0; j < this.r[i].neighbors.Count(); j++)
                    {
                        if (this.r[i].neighbors[j].classification == BasicStructs.TemporaryChain.NeighborsType.neighborTopological)
                        {
                            ret += 1;
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Retorna valor calculado de U
        /// </summary>
        public double U
        {
            get
            {
                double sumValueOfU = 0;

                for (int i = 0; i < this.r.Count; i++)
                {
                    if (this.r[i].e.Count > 0)
                    {
                        for (int j = 0; j < this.r[i].e.Count; j++)
                        {
                            sumValueOfU += this.r[i].e[j].e;
                        }
                    }

                }
                return sumValueOfU;
            }
        }

        /// <summary>
        /// Retorno total de item em E
        /// </summary>
        /// <returns></returns>
        public double ECount()
        {
            int count = 0;

            //foreach (Structs.Point item in r)
            for (int i = 0; i < r.Count; i++)
            {
                BasicStructs.Point item = r[i];
                count += item.e.Count;
            }
            return count;
        }
        /// <summary>
        /// Retorna contatos de primeiro vizinho
        /// </summary>
        public string neighborContacts
        {

            get
            {
                string ret = string.Empty;

                //foreach (Structs.Point item in r)
                for (int i = 0; i < r.Count; i++)
                {
                    BasicStructs.Point item = r[i];
                    if (item.neighborContacts != string.Empty)
                    {
                        ret += item.neighborContacts + ";";
                    }
                }

                return ret;
            }
        }
        #endregion
    }
}
