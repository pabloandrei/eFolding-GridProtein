using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Middle.Helpers.RandomHelpers;
using GridProteinFolding.Core.eFolding.MathsHelpers;
using GICO = GridProteinFolding.Middle.Helpers.IOHelpers.ConsoleOut;
using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Middle.Helpers.LoggingHelpers;

namespace GridProteinFolding.Core.eFolding.MoveSet
{
    public abstract class BaseMoves
    {
        //public abstract bool Do(int selectNode);


        /// <summary>
        /// Aplica o movimento no eixo X
        /// </summary>
        /// <param name="temp">Monomero selecionado para a ação</param>
        internal static void MoveX(ref Structs.BasicStructs.Point temp)
        {

            if (Randomic.randu < 0.5)
                temp.x--;
            else
                temp.x++;

        }

        /// <summary>
        /// Aplica o movimento no eixo Y
        /// </summary>
        /// <param name="temp">Monomero selecionado para a ação</param>
        internal static void MoveY(ref Structs.BasicStructs.Point temp)
        {

            if (Randomic.randu < 0.5)
                temp.y--;
            else
                temp.y++;

        }

        /// <summary>
        /// Aplica o movimento no eixo Z
        /// </summary>
        /// <param name="temp">Monomero selecionado para a ação</param>
        internal static void MoveZ(ref Structs.BasicStructs.Point temp)
        {

            if (Randomic.randu < 0.5)
                temp.z--;
            else
                temp.z++;

        }

        /// <summary>
        /// Verifica se o movimento é valido. A rotina corre todos os Nomomeros da lista e comparação todos os seus Eixos com o Movimento temporário, o qual ainda não foi confirmado.
        /// </summary>
        /// <param name="temp">Monomero o qual será aplicado o movimento</param>
        /// <returns>Return TRUE se o movimento ocorreu</returns>
        internal static bool TryMove(ref Structs.BasicStructs.Point temp)
        {
            //Hint: foreach http://msdn.microsoft.com/en-us/library/ttw7t8t6(VS.80).aspx
            //foreach (Structs.Point checkMonomers in GCPS.chain.r)
            for (int i = 0; i < GCPS.chain.r.Count; i++)
            {
                Structs.BasicStructs.Point checkMonomers = GCPS.chain.r[i];
                //se o Monomero da lista for igual ao temporário, então a ação é cancelada
                if ((checkMonomers.x == temp.x) && (checkMonomers.y == temp.y) && (checkMonomers.z == temp.z))
                    return false;
            }
            return true;
        }

        internal static bool TryMovePeerDPP(ref Structs.BasicStructs.Point temp, Structs.BasicStructs.Point checkMonomers)
        {
            return (Maths4Simulation.DistanceBetweenPointsWithSqrt((Structs.BasicStructs.Point)temp, (Structs.BasicStructs.Point)checkMonomers) == Consts.valueOne);
        }

        internal static bool TryFisrtContact(ref Structs.BasicStructs.Point temp)
        {
            //Verifica se existe contato de primeiro grau
            Structs.BasicStructs.Point tempFor;
            for (int i = GCPS.chain.r.Count + 2; i < GCPS.chain.r.Count; i++)
            {
                tempFor = GCPS.chain.r[i];
                if (Maths4Simulation.DistanceBetweenPointsWithSqrt(tempFor, temp) == Consts.mapContact)
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Testa a existencia de um KINK apartir de uma posição do Monomero
        /// </summary>
        /// <param name="selectNode">Node selecionado</param>
        /// <param name="posInit">Retorna por referência o ponto inicial do Kink</param>
        /// <param name="posEnd">Retorna por referência o ponto final do Kink</param>
        /// <returns>Retorno TRUE se o Kink existir.</returns>
        internal static bool ExistKick(int selectNode, ref Structs.BasicStructs.Point posInit, ref Structs.BasicStructs.Point posEnd)
        {
            try
            {
                //Ponto Vizinho anterior do Monomero sorteado
                posInit = new Structs.BasicStructs.Point() { x = GCPS.chain.r[selectNode - 1].x, y = GCPS.chain.r[selectNode - 1].y, z = GCPS.chain.r[selectNode - 1].z };
                //Ponto Vizinho posterior do Monomero sorteado
                posEnd = new Structs.BasicStructs.Point() { x = GCPS.chain.r[selectNode + 1].x, y = GCPS.chain.r[selectNode + 1].y, z = GCPS.chain.r[selectNode + 1].z };

                //return (GridProteinFolding.Core.Internal.Maths4Simulation.Maths4Simulation.DistanceBetweenPointsWithSqrt(posInit, posEnd) == 1.4142135623730951);

                return Classification.Kink.IsKink(selectNode);
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                GICO.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Sortei qual a posibilidade de moviment entre 1 e lenght
        /// </summary>
        /// <returns></returns>
        internal static int SortPossibilities(int lenght)
        {

            Randomic.Random();

            int ret = (int)(Randomic.randu * lenght);

            //Caso ocorra algum erro no sorteio o algoritmo finaliza a execução
            if (ret < 0 || ret > lenght)
            {
                new GridProteinFolding.Middle.Helpers.LoggingHelpers.Log().ErroParseMoves(new Middle.Helpers.LoggingHelpers.User_Defined_Exceptions.ErroParseMovesException("SortCrankShaftMove"), Types.ErrorLevel.None);

                throw new System.Exception();
                //CustomMessage customMessage = new CustomMessage();

                //throw new CustomException(SeverityLevel.Critical,
                //    LogLevel.Event,
                //    new ErroParseMoves("SortCrankShaftMove Exception"),
                //   customMessage.GetString("SortCrankShaftMove"));

            }
            else
                return ret;

        }
    }
}
