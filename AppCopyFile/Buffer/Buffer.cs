using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppCopyFile
{
    public class Buffer : IBuffer, IProgress
    {
        private class InnerBuffer
        {
            public byte[] buffer;
            public int sizeData;
            public InnerBuffer(int sizeBuffer)
            {
                buffer = new byte[sizeBuffer];
            }

            public void Write(FileStream fileStream, out int written)
            {
                written = fileStream.Read(buffer, 0, buffer.Length);
                sizeData = written;
            }

            public void Read(FileStream fileStream, out int readed)
            {
                fileStream.Write(buffer, 0, sizeData);
                readed = sizeData;
            }
        }
        private InnerBuffer[] buffer;
        private object obj = new object();
        private ConcurrentQueue<int> readQueue;
        private ConcurrentQueue<int> writeQueue;

        public event Action<long> ProgressChanged;

        public Buffer(int sizeBuffer, int buffersCount)
        {
            readQueue = new ConcurrentQueue<int>();
            writeQueue = new ConcurrentQueue<int>();

            foreach (var number in Enumerable.Range(0, buffersCount - 1))
            {
                writeQueue.Enqueue(number);
            }

            buffer = Enumerable.Repeat(sizeBuffer, buffersCount).Select(a=> new InnerBuffer(sizeBuffer)).ToArray();
        }

        public bool TryCopyFrom(FileStream fileStream, out int readed)
        {
            lock (obj)
            {
                int bufferIndex;
                if (readQueue.TryPeek(out bufferIndex))
                {
                    buffer[bufferIndex].Read(fileStream, out readed);
                    if (readed > 0)
                    {
                        readQueue.TryDequeue(out bufferIndex);
                        writeQueue.Enqueue(bufferIndex);
                        ProgressChanged?.Invoke(readQueue.Count() * 100 / buffer.Length);

                        return true;
                    }

                    return false;
                }
                else
                {
                    readed = 0;
                    return false;
                }
            }
        }

        public bool TryCopyTo(FileStream fileStream, out int written)
        {
            lock (obj)
            {
                int bufferIndex;
                if (writeQueue.TryPeek(out bufferIndex))
                {
                    buffer[bufferIndex].Write(fileStream, out written);
                    if (written > 0)
                    {
                        writeQueue.TryDequeue(out bufferIndex);
                        readQueue.Enqueue(bufferIndex);
                        ProgressChanged?.Invoke(readQueue.Count() * 100 / buffer.Length);
                        return true;
                    }
                    return false;
                }
                else
                {
                    written = 0;
                    return false;
                }
            }
        }
    }
}
