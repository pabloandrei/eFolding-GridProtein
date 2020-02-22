using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridProteinFolding.Middle.Helpers.CryptographyHelpers
{
    public static class KeyGen
    {
        //public static byte Add(this byte a, byte b)
        //{
        //    return (byte)(a + b);
        //}

        public static string Generator(Guid guid)
        {
            byte[] key = guid.ToByteArray();

            //byte[] newKey = new byte[4];

            //for (int i = 0; i < 4; i++)
            //{
            //    newKey[i] = Add(Add(key[i], key[i + 4]), Add(key[i + 8], key[i + 12]));
            //}

            //return newKey[0].ToString() + newKey[1].ToString() + newKey[2].ToString() + newKey[3].ToString();

            return "eFolding";
        }
    }
}
