//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//

//namespace GridProteinFolding.ProcessHelpers.Parse
//{
//    /// <summary>
//    /// Classe responsável pela verificação Binária de entre arquivos
//    /// </summary>
//    internal class CheckFile
//    {
//        const int BYTES_TO_READ = sizeof(Int64);

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="firstFile"></param>
//        /// <param name="secondFile"></param>
//        /// <returns></returns>
//        internal static bool FilesAreEqual(string firstFile, string secondFile)
//        {
//            FileInfo first = new FileInfo(Internal.Stream.Stream.dirToWorkOut + @"\" + firstFile);
//            FileInfo second = new FileInfo(Internal.Stream.Stream.dirToWorkOut + @"\" + secondFile);
//            bool ret = true;

//            if (first.Length != second.Length)
//                return false;

//            int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

//            using (ExtendedFileStream fs1 = first.OpenRead())
//            using (ExtendedFileStream fs2 = second.OpenRead())
//            {
//                byte[] one = new byte[BYTES_TO_READ];
//                byte[] two = new byte[BYTES_TO_READ];

//                for (int i = 0; i < iterations; i++)
//                {
//                    fs1.Read(one, 0, BYTES_TO_READ);
//                    fs2.Read(two, 0, BYTES_TO_READ);

//                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
//                        ret = false;
//                }
//            }
                     

//            first = null;
//            second = null;

//            return ret;

//        }
//    }
//}
