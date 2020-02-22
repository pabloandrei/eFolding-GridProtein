using System;
using System.Runtime.InteropServices;

namespace GridProteinFolding.Core.eFolding
{
    public static class Marshalling
    {
        public static byte[] getBytes(Simulation simulation)
        {
            int size = Marshal.SizeOf(simulation);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(simulation, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public static Simulation fromBytes(byte[] arr)
        {
            Simulation simulation = new Simulation();

            int size = Marshal.SizeOf(simulation);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            simulation = (Simulation)Marshal.PtrToStructure(ptr, simulation.GetType());
            Marshal.FreeHGlobal(ptr);

            return simulation;
        }

        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the XML file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            //using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            //{
            //    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //    binaryFormatter.Serialize(stream, objectToWrite);
            //}
        }
    }
}
