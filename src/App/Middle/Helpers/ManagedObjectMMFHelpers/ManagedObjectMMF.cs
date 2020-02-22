using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;

namespace GridProteinFolding.Middle.Helpers.ManagedObjectMMFHelpers
{
    public class ManagedObjectMMF
    {
        #region Generic MMF read/write object functions

        public static void WriteObjectToMMF(string mmfFile, object objectData, bool append)
        {
            // Convert .NET object to byte array
            byte[] buffer = ObjectToByteArray(objectData);

            // Create a new memory mapped file
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(mmfFile, append ? FileMode.Append : FileMode.Create, null, buffer.Length))
            {
                // Create a view accessor into the file to accommmodate binary data size
                using (MemoryMappedViewAccessor mmfWriter = mmf.CreateViewAccessor(0, buffer.Length))
                {
                    // Write the data
                    mmfWriter.WriteArray<byte>(0, buffer, 0, buffer.Length);
                }
            }
        }

        public static object ReadObjectFromMMF(string mmfFile)
        {
            // Get a handle to an existing memory mapped file
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(mmfFile, FileMode.Open))
            {
                // Create a view accessor from which to read the data
                using (MemoryMappedViewAccessor mmfReader = mmf.CreateViewAccessor())
                {
                    // Create a data buffer and read entire MMF view into buffer
                    byte[] buffer = new byte[mmfReader.Capacity];
                    mmfReader.ReadArray<byte>(0, buffer, 0, buffer.Length);

                    // Convert the buffer to a .NET object
                    return ByteArrayToObject(buffer);
                }
            }
        }

        #endregion

        #region Object/Binary serialization

        static object ByteArrayToObject(byte[] buffer)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();    // Create new BinaryFormatter
            MemoryStream memoryStream = new MemoryStream(buffer);       // Convert byte array to memory stream, set position to start
            return binaryFormatter.Deserialize(memoryStream);           // Deserializes memory stream into an object and return
        }

        static byte[] ObjectToByteArray(object inputObject)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();    // Create new BinaryFormatter
            MemoryStream memoryStream = new MemoryStream();             // Create target memory stream
            binaryFormatter.Serialize(memoryStream, inputObject);       // Convert object to memory stream
            return memoryStream.ToArray();                              // Return memory stream as byte array
        }

        #endregion
    }
}
