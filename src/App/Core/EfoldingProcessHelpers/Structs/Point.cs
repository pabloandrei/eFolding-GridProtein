using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Core.eFolding.Structs
{
    public partial class BasicStructs
    {
        /// <summary>
        /// Estrutura que defini informações dos eixos X,Y,Z para um ponto qualquer.
        /// </summary>
        public struct Point
        {
            /// <summary>
            /// Atributo que defini valor para X
            /// </summary>
            public int x;
            /// <summary>
            /// Atributo que defini valor para Y
            /// </summary>
            public int y;
            /// <summary>
            /// Atributo que defini valor para Z
            /// </summary>
            public int z;
            public int deadEnd;
            public string deadEndPoints;
            public ClassificationMotion classificationMotion;
            public Neighbor[] neighbors;

            //public string ToStringCrank() {

            //    return "R0:" + Convert.ToString(classificationMotion.crankShaft__R0) +
            //        "R1:" + Convert.ToString(classificationMotion.crankShaft__R1) +
            //        "R2:" + Convert.ToString(classificationMotion.crankShaft__R2) +
            //        "R3:" + Convert.ToString(classificationMotion.crankShaft__R3);
            //}

            /// <summary>
            /// Valor atual de (e), para residuo dentro da matriz energetica  
            /// </summary>          
            public List<TypeE> e;

            /// <summary>
            /// Retorna coleção (formatada em uma única string) dos contatos topológicos de primeiro vizinhos
            /// </summary>
            public string neighborContacts
            {
                get
                {
                    string ret = string.Empty;
                    for (int i = 0; i < e.Count; i++)
                    {
                        ret += "{" + e[i].selectNode + "," + e[i].i + "}";
                    }

                    return ret;
                }
            }
        }

        ///// <summary>
        ///// Class Model
        ///// </summary>
        //public class Model
        //{

        //    byte monomero;

        //    /// <summary>
        //    /// Monomero
        //    /// </summary>
        //    public byte Monomero
        //    {
        //        get { return monomero; }
        //        set { monomero = value; }
        //    }
        //    float value;

        //    /// <summary>
        //    /// Valor
        //    /// </summary>
        //    public float Value
        //    {
        //        get { return this.value; }
        //        set { this.value = value; }
        //    }

        //    /// <summary>
        //    /// Construtor da Class Model
        //    /// </summary>
        //    public Model() { }

        //    /// <summary>
        //    /// Construtor da Class Model
        //    /// </summary>
        //    /// <param name="monomero"></param>
        //    /// <param name="value"></param>
        //    public Model(byte monomero, float value)
        //    {

        //        this.monomero = monomero;
        //        this.value = value;

        //    }

        //}
    }
}
