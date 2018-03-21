using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace AppCopyFile
{
    public class FileReader : IFileReader, IDisposable, IProgress
    {
        private Timer timer;
        private IBuffer buffer;
        private FileStream fileStream;
        private long offset;
        private int dueTime;
        private int period;

        public FileReader(string pathFile, IBuffer buffer, int period)
        {
            dueTime = 0;
            this.period = period;
            this.buffer = buffer;
            offset = 0;
            fileStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read);
            timer = new Timer(ReaderFile, null, -1, -1);
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

        private void ReaderFile(object state)
        {
            try
            {
                if (buffer.TryCopyTo(fileStream, out int written) && written > 0)
                {
                    offset = offset + written;
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
