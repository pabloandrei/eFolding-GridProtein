using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCPS = GridProteinFolding.Core.eFolding.Simulation;
using GridProteinFolding.Core.eFolding;

namespace GridProteinFolding.Core.eFolding.Structs
{
    /// <summary>
    /// FakeProtein
    /// </summary>
    public class FakeProtein : IDisposable
    {
        /// <summary>
        /// Metodo Dispose da Classe
        /// </summary>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cria um monomero FAKE para teste
        /// </summary>
        public static void FakeCadeia()
        {

            //    Members.monomero.r.Add(new Structs.Point() { x = 50, y = 50, z = 50 }); //0
            //    Members.monomero.r.Add(new Structs.Point() { x = 51, y = 50, z = 50 }); //1
            //    Members.monomero.r.Add(new Structs.Point() { x = 51, y = 51, z = 50 }); //2
            //    Members.monomero.r.Add(new Structs.Point() { x = 52, y = 51, z = 50 }); //3
            //    Members.monomero.r.Add(new Structs.Point() { x = 53, y = 51, z = 50 }); //4
            //    Members.monomero.r.Add(new Structs.Point() { x = 53, y = 50, z = 50 }); //5
            //    Members.monomero.r.Add(new Structs.Point() { x = 54, y = 50, z = 50 }); //6
            //    Members.monomero.r.Add(new Structs.Point() { x = 55, y = 50, z = 50 }); //7
            //    Members.monomero.r.Add(new Structs.Point() { x = 56, y = 50, z = 50 }); //8
            //    Members.monomero.r.Add(new Structs.Point() { x = 57, y = 50, z = 50 }); //9
            //    Members.monomero.r.Add(new Structs.Point() { x = 57, y = 51, z = 50 }); //10
            //    Members.monomero.r.Add(new Structs.Point() { x = 58, y = 51, z = 50 }); //11
            //    Members.monomero.r.Add(new Structs.Point() { x = 58, y = 50, z = 50 }); //12
            //    Members.monomero.r.Add(new Structs.Point() { x = 59, y = 50, z = 50 }); //13
            //    Members.monomero.r.Add(new Structs.Point() { x = 60, y = 50, z = 50 }); //14
            //    Members.monomero.r.Add(new Structs.Point() { x = 61, y = 50, z = 50 }); //15
            //    Members.monomero.r.Add(new Structs.Point() { x = 61, y = 51, z = 50 }); //16
            //    Members.monomero.r.Add(new Structs.Point() { x = 62, y = 51, z = 50 }); //17
            //    Members.monomero.r.Add(new Structs.Point() { x = 62, y = 50, z = 50 }); //18
            //    Members.monomero.r.Add(new Structs.Point() { x = 63, y = 50, z = 50 }); //19
            //    Members.monomero.r.Add(new Structs.Point() { x = 64, y = 50, z = 50 }); //20
            //    Members.monomero.r.Add(new Structs.Point() { x = 65, y = 50, z = 50 }); //21
            //    Members.monomero.r.Add(new Structs.Point() { x = 66, y = 50, z = 50 }); //22
            //    Members.monomero.r.Add(new Structs.Point() { x = 67, y = 50, z = 50 }); //23
            //    Members.monomero.r.Add(new Structs.Point() { x = 68, y = 50, z = 50 }); //24
            //    Members.monomero.r.Add(new Structs.Point() { x = 71, y = 50, z = 50 }); //25
            //    Members.monomero.r.Add(new Structs.Point() { x = 72, y = 50, z = 50 }); //26

            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 51, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 52, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 53, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 54, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 55, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 56, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 57, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 58, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 59, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 60, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 61, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 62, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 63, y = 51, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 63, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 62, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 61, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 60, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 59, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 58, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 57, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 56, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 55, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 54, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 53, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 52, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 51, y = 52, z = 51 });
            GCPS.chain.r.Add(new Structs.BasicStructs.Point() { x = 50, y = 52, z = 51 });
        }
    }
}
