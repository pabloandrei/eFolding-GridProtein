using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GridProteinFolding.Middle.Helpers.IOHelpers
{
    //public class CustomStream : Stream
    //{
    //    private string dataStream = string.Empty;
    //    private long position = 0;

    //    public override bool CanRead
    //    {
    //        get { return true; }
    //    }

    //    public override bool CanSeek
    //    {
    //        get { return false; }
    //    }

    //    public override bool CanWrite
    //    {
    //        get { return false; }
    //    }


    //    public override long Length
    //    {
    //        get { return dataStream.Length; }
    //    }

    //    public override long Position
    //    {
    //        get
    //        {
    //            return position;
    //        }
    //        set
    //        {
    //            position = value < this.Length ? value : this.Length;
    //        }
    //    }

    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        long bufferPos = offset;
    //        long max = this.position + count;
    //        max = max < this.Length ? max : this.Length;
    //        for (; this.position < max; this.position++)
    //        {
    //            buffer[bufferPos] = Convert.ToByte(this.dataStream[(int)this.position]);
    //            bufferPos++;
    //        }

    //        return (int)bufferPos - offset;
    //    }

    //    public override void Flush()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override long Seek(long offset, SeekOrigin origin)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void SetLength(long value)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Write(byte[] buffer, int offset, int count)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class ExtendedStream : Stream
    //{
    //    private readonly Stream internalStream;
    //    private readonly Action<byte[]> _readCallback;

    //    private ManualResetEvent dataInjected = new ManualResetEvent(false);
    //    private List<byte> data = new List<byte>();
    //    private int pos = 0;

    //    public ExtendedStream(Stream originalStream, Action<byte[]> readCallback)
    //    {
    //        internalStream = originalStream;
    //        _readCallback = readCallback;
    //    }

    //    public ExtendedStream(Stream originalStream)
    //    {
    //        internalStream = originalStream;
    //    }

    //    //public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
    //    //{
    //    //    var read = await _originalStream.ReadAsync(buffer, offset, count, cancellationToken);

    //    //    _readCallback(buffer);

    //    //    return read;
    //    //}

    //    public void Inject(string text)
    //    {
    //        data.AddRange(new UTF8Encoding(false).GetBytes(text));
    //        dataInjected.Set();
    //    }

    //    private IEnumerable<byte> GetBytes(int count)
    //    {
    //        int returned = 0;

    //        while (returned == 0)
    //        {
    //            if (pos < data.Count)
    //            {
    //                while (pos < data.Count && returned < count)
    //                {
    //                    yield return data[pos];

    //                    pos += 1; returned += 1;
    //                }
    //            }
    //            else
    //            {
    //                dataInjected.Reset();
    //                dataInjected.WaitOne();
    //            }
    //        }
    //    }

    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        var bytes = GetBytes(count).ToArray();

    //        for (int i = 0; offset + i < buffer.Length && i < bytes.Length; i++)
    //        {
    //            buffer[offset + i] = bytes[i];
    //        }

    //        return bytes.Length;
    //    }

    //    public override void Write(byte[] buffer, int offset, int count)
    //    {
    //        internalStream.Write(buffer, offset, count);
    //    }

    //    public override bool CanRead
    //    {
    //        get { return internalStream.CanRead; }
    //    }

    //    public override bool CanSeek
    //    {
    //        get { return internalStream.CanSeek; }
    //    }

    //    public override bool CanWrite
    //    {
    //        get { return internalStream.CanWrite; }
    //    }

    //    public override void Flush()
    //    {
    //        internalStream.Flush();
    //    }

    //    public override long Length
    //    {
    //        get { return internalStream.Length; }
    //    }

    //    public override long Position
    //    {
    //        get { return internalStream.Position; }
    //        set { internalStream.Position = value; }
    //    }

    //    public override long Seek(long offset, SeekOrigin origin)
    //    {
    //        return internalStream.Seek(offset, origin);
    //    }

    //    public override void SetLength(long value)
    //    {
    //        internalStream.SetLength(value);
    //    }
    //}

}
//    //Then I am reading the stream with XmlReader like follows.

//    //using (XmlReader xmlReader = XmlReader.Create(_extendedStream, new XmlReaderSettings() { Async = true }))
//    //{
//    //                while (await xmlReader.ReadAsync())
//    //                {                        
//    //                    switch (xmlReader.NodeType)
//    //                    {                            
//    //                        case XmlNodeType.EndElement:
//    //                            if (xmlReader.LocalName.Equals("test"))
//    //                            {
//    //                                _log.Debug("</test> injected!");                            
//    //                            }
//    //                            break;
//    //                        default:                              
//    //                            break;
//    //                    }
//}
