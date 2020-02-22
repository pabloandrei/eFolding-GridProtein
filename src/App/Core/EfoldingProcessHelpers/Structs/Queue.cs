using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Core.eFolding.Structs
{
    /// <summary>
    /// Queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Queue<T> : System.Collections.Generic.Queue<T>, IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        //DBCompact.Base objBase = new DBCompact.Base();

        /// <summary>
        /// (1)- cadeia ideal ( sem qualquer restricao); 
        /// </summary>
        public static Queue<QueueType> valuesCadeiaIdeal = new Queue<QueueType>();

        /// <summary>
        /// (2)- cadeia real (ou cadeia com volume excluido, isto é, sem suporposicao)
        /// </summary>
        public static Queue<QueueType> valuesCadeiaReal = new Queue<QueueType>();

        /// <summary>
        /// (3)- cadeia soft (sem superposicao e sem vizinho topologico)
        /// </summary>
        public static Queue<QueueType> valuesCadeiaSoft = new Queue<QueueType>();



        /// <summary>
        /// Enqueue item T na fila
        /// </summary>
        /// <param name="item"></param>
        new public void Enqueue(T item)
        {
            base.Enqueue(item);
        }


        /// <summary>
        /// Dequeue fila de T
        /// </summary>
        /// <returns></returns>
        new public T Dequeue()
        {
            //T temp = base.Dequeue();

            //objBase.AppendQueue(temp);

            //return temp;
            return base.Dequeue();
        }
    }

    /// <summary>
    /// QueueType
    /// </summary>
    public class QueueType
    {
        /// <summary>
        /// Nome do Isem
        /// </summary>
        public string isemName;
        /// <summary>
        /// Valor do raio de giração
        /// </summary>
        public double spinningRayValue;
        /// <summary>
        /// Vaslor do cálculo de distancia ponta a ponta
        /// </summary>
        public double calDistanceBetweenLastPointFirst;

        public QueueType(string isemName, double spinningRayValue, double calDistanceBetweenLastPointFirst)
        {
            this.isemName = isemName;
            this.spinningRayValue = spinningRayValue;
            this.calDistanceBetweenLastPointFirst = calDistanceBetweenLastPointFirst;
        }
    }
}
