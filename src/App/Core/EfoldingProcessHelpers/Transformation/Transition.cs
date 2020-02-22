using GridProteinFolding.Core.eFolding.Structs;
using GridProteinFolding.Middle.Helpers.IOHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Core.eFolding.Transformation
{
    public class Transition
    {
        public static List<int> DoTransLastTrajectoryToNative(List<BasicStructs.Point> trajectoryToStruct, string nameFileGhostCube)
        {
            //Efetua a transição entre uma Trajetoria e uma Estrutura
            trajectoryToStruct = TransitionTrajectoryToStructs(trajectoryToStruct); //Trajectory to Ghost
            return NativeStruct.DoGhostStructToNativeStruct(trajectoryToStruct, nameFileGhostCube); //Ghost to Native
        }


        private static void MaxMin(BasicStructs.Point temp, ref int majorX, ref int minusX, ref int majorY, ref int minusY, ref int majorZ, ref int minusZ)
        {
            //check dif em relação as extremidades X
            majorX = Math.Max(majorX, temp.x);
            minusX = Math.Min(minusX, temp.x);

            //check dif em relação as extremidades Y
            majorY = Math.Max(majorY, temp.y);
            minusY = Math.Min(minusY, temp.y);

            //check dif em relação as extremidades Z
            majorZ = Math.Max(majorZ, temp.z);
            minusZ = Math.Min(minusZ, temp.z);
        }

        private static List<BasicStructs.Point> TransitionTrajectoryToStructs(List<BasicStructs.Point> points)
        {
            List<BasicStructs.Point> ret = new List<BasicStructs.Point>();

            int difX = points[0].x;
            int difY = points[0].y;
            int difZ = points[0].z;

            int majorX = 0, minusX = 0;
            int majorY = 0, minusY = 0;
            int majorZ = 0, minusZ = 0;

            //Transition
            for (int i = 0; i < points.Count(); i++)
            {
                BasicStructs.Point temp = new BasicStructs.Point();
                temp.x = points[i].x - difX;
                temp.y = points[i].y - difY;
                temp.z = points[i].z - difZ;

                ret.Add(temp);

                //check dif em relação as extremidades X,Y,Z
                MaxMin(temp, ref majorX, ref minusX, ref majorY, ref minusY, ref majorZ, ref minusZ);
            }

            //Console.WriteLine("X:{0},{1} Y:{2},{3} Z:{4},{5}", minusX, majorX, minusY, majorY, minusZ, majorZ);
            int difPosX = (minusX + majorX), difPosY = (minusY + majorY), difPosZ = (minusZ + majorZ);
            //Console.WriteLine("X:{0} Y:{1} Z:{2}", difPosX, difPosY, difPosZ);
            //Console.WriteLine();

            if ((difPosX != 2) || (difPosY != 2) || (difPosZ != 2))
            {
                ret = TransitionTrajectoryToStructsWithBaias(ret, difPosX, difPosY, difPosZ);
            }

            return ret;
        }


        private static List<BasicStructs.Point> TransitionTrajectoryToStructsWithBaias(List<BasicStructs.Point> points, int difX, int difY, int difZ)
        {
            //corrige baias
            if (difX == 2)
                difX = 0;
            else
                difX = -1;

            if (difY == 2)
                difY = 0;
            else
                difY = -1;

            if (difZ == 2)
                difZ = 0;
            else
                difZ = -1;

            List<BasicStructs.Point> ret = new List<BasicStructs.Point>();

            int majorX = 0, minusX = 0;
            int majorY = 0, minusY = 0;
            int majorZ = 0, minusZ = 0;

            //Transition
            for (int i = 0; i < points.Count(); i++)
            {
                BasicStructs.Point temp = new BasicStructs.Point();
                temp.x = points[i].x - difX;
                temp.y = points[i].y - difY;
                temp.z = points[i].z - difZ;
                ret.Add(temp);

                //check dif em relação as extremidades X,Y,Z
                MaxMin(temp, ref majorX, ref minusX, ref majorY, ref minusY, ref majorZ, ref minusZ);
            }


            //Console.WriteLine("\t X:{0},{1} Y:{2},{3} Z:{4},{5}", minusX, majorX, minusY, majorY, minusZ, majorZ);
            //int difPosX = (minusX + majorX), difPosY = (minusY + majorY), difPosZ = (minusZ + majorZ);
            //Console.WriteLine("\t X:{0} Y:{1} Z:{2}", difPosX, difPosY, difPosZ);
            //Console.WriteLine();

            return ret;
        }

    }
}
