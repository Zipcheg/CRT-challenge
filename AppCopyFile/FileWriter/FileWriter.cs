using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace AppCopyFile
{
    public class FileWriter : IFileWriter, IDisposable, IProgress
    {
        private Timer timer;
        private IBuffer buffer;
        private FileStream fileStream;
        private long offset;
        private int dueTime;
        private int period;

        public FileWriter(string pathFile, IBuffer buffer, int period)
        {
            dueTime = 0;
            this.period = period;
            this.buffer = buffer;
            offset = 0;
            fileStream = new FileStream(pathFile, FileMode.OpenOrCreate, FileAccess.Write);
            timer = new Timer(WriteFile, null, -1, -1);
        }

        public event Action<long> ProgressChanged;

        public void Dispose()
        {
            fileStream.Dispose();
            timer.Dispose();
        }

        public void Pause()
        {
            timer.Change(-1, -1);
        }

        public void Start()
        {
            timer.Change(dueTime, period);
        }

        private void WriteFile(object state)
        {
            try
            {
                if (buffer.TryCopyFrom(fileStream, out int readed) && readed > 0)
                {
                    offset = offset + readed;
                    ProgressChanged?.Invoke(offset);
                }
            }
            catch (Exception ex)
            {
                Pause();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
