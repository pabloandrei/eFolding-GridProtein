using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Core.eFolding.Structs
{
    public partial class BasicStructs
    {
        BasicStructs()
        {
        }

        //int minValue = int.MinValue;

        /// <summary>
        /// Estrutura que classifica qual o tipo de movimento possível por monomero
        /// </summary>
        public struct ClassificationMotion
        {
            /// <summary>
            /// Quando TRUE indica que foi classificado como ENDS, o tipo de movimento possível do monomero
            /// </summary>
            public bool ends;
            /// <summary>
            /// Quando TRUE indica que foi classificado como KINK, o tipo de movimento possível do monomero
            /// </summary>
            public bool kink;
            /// <summary>
            /// Quando TRUE indica que foi classificado como crankShaft__R0, o tipo de movimento possível do monomero
            /// </summary>
            public bool crankShaft__R0;
            /// <summary>
            /// Quando TRUE indica que foi classificado como crankShaft__R1, o tipo de movimento possível do monomero
            /// </summary>
            public bool crankShaft__R1;
            /// <summary>
            /// Quando TRUE indica que foi classificado como crankShaft__R2, o tipo de movimento possível do monomero
            /// </summary>
            public bool crankShaft__R2;
            /// <summary>
            /// Quando TRUE indica que foi classificado como crankShaft__R3, o tipo de movimento possível do monomero
            /// </summary>
            public bool crankShaft__R3;
            /// <summary>
            /// Quando TRUE indica que foi classificado como Stretched, o tipo de movimento possível do monomero
            /// </summary>
            public bool stretched;

        }

        /// <summary>
        /// Estrutura de classificação dos vizinhos
        /// </summary>
        public struct Neighbor
        {
            /// <summary>
            /// Classficação do resíduo
            /// </summary>
            public TemporaryChain.NeighborsType classification;
            /// <summary>
            /// Indice do resíduo de contato
            /// </summary>
            public int contacResidue;

        }


        /// <summary>
        /// Estrutura que representa as informações de quantos movimentos ocorreram para os trÊs (3) tipos de movimentos possíveis.
        /// </summary>
        public struct MoveSetApplies
        {
            /// <summary>
            /// Total de movimentos: Aceitos +Rejeitados
            /// </summary>
            public Int64 SumOfAcceptedAndRejectedMovement
            {
                get { return NumberOfMovementsApplied + NumberOfMovementsRejected; }
            }

            /// <summary>
            /// Total de movimento: Aceitos
            /// </summary>
            public Int64 NumberOfMovementsApplied
            {
                get { return (crankshaftAccept + endsAccept + kinkAccept); }
            }

            /// <summary>
            /// Total de movimento: Rejeitados
            /// </summary>
            public Int64 NumberOfMovementsRejected
            {
                get { return (crankshaftReject + endsReject + kinkReject + othersReject); }
            }

            /// <summary>
            /// Total de movimentos Crankshaft ocorridos
            /// </summary>
            public Int64 TotalCankshaftAttempts
            {
                get { return crankshaftAccept + crankshaftReject; }
            }

            /// <summary>
            /// Total de movimentos Kink ocorridos
            /// </summary>
            public Int64 TotatKinKAttempts
            {
                get { return kinkAccept + kinkReject; }
            }

            /// <summary>
            /// Total de movimentos End ocorridos
            /// </summary>
            public Int64 TotalEndAttempts
            {
                get { return endsAccept + endsReject; }
            }

            /// <summary>
            /// Movimento Crankshaft aceitos
            /// </summary>
            public Int64 crankshaftAccept;

            /// <summary>
            /// Movimento de Ends aceitos
            /// </summary>
            public Int64 endsAccept;

            /// <summary>
            /// Movimentos de Kink aceitos
            /// </summary>
            public Int64 kinkAccept;

            /// <summary>
            /// Movimentos de Crankshaft rejeitados
            /// </summary>
            public Int64 crankshaftReject;

            /// <summary>
            /// Movimentos de Ends rejeitados
            /// </summary>
            public Int64 endsReject;

            /// <summary>
            /// Movimentos de Kink rejeitados
            /// </summary>
            public Int64 kinkReject;

            /// <summary>
            /// Movimentos rejeitados
            /// </summary>
            public Int64 othersReject;

            /// <summary>
            /// MoveSetCrankshaft
            /// </summary>
            public MoveSetCrankshaft moveSetCrankshaft;
            /// <summary>
            /// MoveSetEnd
            /// </summary>
            public MoveSetEnd moveSetEnd;
            /// <summary>
            /// MoveSetKink
            /// </summary>
            public MoveSetKink moveSetKink;

        }


        /// <summary>
        /// SubGrupo de controle (contador) dos movimentos possiveis dentro de uma KINK por EIXO
        /// </summary>
        public struct MoveSetKink
        {
            //public Int64 kink_x;
            //public Int64 kink_y;
            //public Int64 kink_z;            

        }

        /// <summary>
        /// SubGrupo de controle (contador) dos movimentos possiveis dentro de uma END por EIXO
        /// </summary>
        public struct MoveSetEnd
        {
            /// <summary>
            /// Total de movimentos aceitos
            /// </summary>
            public Int64 TotalAccept
            {
                get { return endAccept_1 + endAccept_2 + endAccept_3 + endAccept_4; }
            }

            /// <summary>
            /// Movimento End aceitos tipo 1
            /// </summary>
            public Int64 endAccept_1;
            /// <summary>
            /// Movimento End aceitos tipo 2
            /// </summary>
            public Int64 endAccept_2;
            /// <summary>
            /// Movimento End aceitos tipo 3
            /// </summary>
            public Int64 endAccept_3;
            /// <summary>
            /// Movimento End aceitos tipo 4
            /// </summary>
            public Int64 endAccept_4;

            /// <summary>
            /// Total de movimentos rejeitados
            /// </summary>
            public Int64 TotalReject
            {
                get { return endReject_1 + endReject_2 + endReject_3 + endReject_4; }
            }

            /// <summary>
            /// Movimento End rejeitados tipo 1
            /// </summary>
            public Int64 endReject_1;
            /// <summary>
            /// Movimento End rejeitados tipo 2
            /// </summary>
            public Int64 endReject_2;
            /// <summary>
            /// Movimento End rejeitados tipo 3
            /// </summary>
            public Int64 endReject_3;
            /// <summary>
            /// Movimento End rejeitados tipo 4
            /// </summary>
            public Int64 endReject_4;

            /// <summary>
            /// Movimento End totalizados tipo 1
            /// </summary>
            public Int64 endTotal_1
            {
                get { return endAccept_1 + endReject_1; }
            }

            /// <summary>
            /// Movimento End totalizados tipo 2
            /// </summary>
            public Int64 endTotal_2
            {
                get { return endAccept_2 + endReject_2; }
            }

            /// <summary>
            /// Movimento End totalizados tipo 3
            /// </summary>
            public Int64 endTotal_3
            {
                get { return endAccept_3 + endReject_3; }
            }

            /// <summary>
            /// Movimento End totalizados tipo 4
            /// </summary>
            public Int64 endTotal_4
            {
                get { return endAccept_4 + endReject_4; }
            }
        }

        /// <summary>
        /// SubGrupo de controle (contador) dos movimentos possiveis dentro de uma MANIVELA por EIXO
        /// </summary>
        public struct MoveSetCrankshaft
        {
            /// <summary>
            /// Total de movimentos aceitos
            /// </summary>
            public Int64 TotalAccept
            {
                get { return crankshaftAccept_1 + crankshaftAccept_2 + crankshaftAccept_3; }
            }

            /// <summary>
            /// Movimento Crankshaft aceitos tipo 1
            /// </summary>
            public Int64 crankshaftAccept_1;
            /// <summary>
            /// Movimento Crankshaft aceitos tipo 2
            /// </summary>
            public Int64 crankshaftAccept_2;
            /// <summary>
            /// Movimento Crankshaft aceitos tipo 3
            /// </summary>
            public Int64 crankshaftAccept_3;

            /// <summary>
            /// Total de nmovimentos rejeitados
            /// </summary>
            public Int64 TotalReject
            {
                get { return crankshaftReject_1 + crankshaftReject_2 + crankshaftReject_3; }
            }

            /// <summary>
            /// Movimento Crankshaft rejeitados tipo 1
            /// </summary>
            public Int64 crankshaftReject_1;
            /// <summary>
            /// Movimento Crankshaft rejeitados tipo 2
            /// </summary>
            public Int64 crankshaftReject_2;
            /// <summary>
            /// Movimento Crankshaft rejeitados tipo 3
            /// </summary>
            public Int64 crankshaftReject_3;
            /// <summary>
            /// Movimento Crankshaft aceitos tipo 1
            /// </summary>
            public Int64 crankshaftTotal_1
            {
                get { return crankshaftAccept_1 + crankshaftReject_1; }
            }
            /// <summary>
            /// Movimento Crankshaft aceitos tipo 2
            /// </summary>
            public Int64 crankshaftTotal_2
            {
                get { return crankshaftAccept_2 + crankshaftReject_2; }
            }
            /// <summary>
            /// Movimento Crankshaft aceitos tipo 3
            /// </summary>
            public Int64 crankshaftTotal_3
            {
                get { return crankshaftAccept_3 + crankshaftReject_3; }
            }
        }

    }

}
