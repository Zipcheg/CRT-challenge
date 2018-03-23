using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Input;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace AppCopyFile
{
    public class CopyProcessViewModel : BindableBase, IInitializable
    {
        private Buffer buffer;
        private FileWriter fileWriter;
        private FileReader fileReader;


        private StateButtom stateButtomReadStream;
        private StateButtom stateButtomWriteStream;

        private readonly DelegateCommand readStreamButtomCommand;
        private readonly DelegateCommand writeStreamButtomCommand;

        private string nameReadStreamButtom;
        private string nameWriteStreamButtom;
        private string colorStreamWriteBuffer;
        private string colorStreamWriteFile;

        private int periodRead;
        private int periodWrite;
        private int percentageBuffer;
        private int percentageWriteStream;
        private int sizeBuffer;

        private bool isEnabled;

        private long fileSize;

        public CopyProcessViewModel()
        {
            IsEnabled = true;

            nameReadStreamButtom = "Приостановить";
            nameWriteStreamButtom = "Приостановить";

            ColorStreamWriteBuffer = "Green";
            ColorStreamWriteFile = "Green";

            readStreamButtomCommand = new DelegateCommand(PauseUnpauseReadStream);
            writeStreamButtomCommand = new DelegateCommand(PauseUnpauseWriteStream);

            stateButtomReadStream = StateButtom.Pause;
            stateButtomWriteStream = StateButtom.Pause;

        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                SetProperty(ref isEnabled, value);
            }
        }

        public string ColorStreamWriteBuffer
        {
            get
            {
                return colorStreamWriteBuffer;
            }
            set
            {
                SetProperty(ref colorStreamWriteBuffer, value);
            }
        }

        public string ColorStreamWriteFile
        {
            get
            {
                return colorStreamWriteFile;
            }
            set
            {
                SetProperty(ref colorStreamWriteFile, value);
            }
        }

        public string NameReadStreamButtom
        {
            get
            {
                return nameReadStreamButtom;
            }

            set
            {
                nameReadStreamButtom = value;

                RaisePropertyChanged(nameof(NameReadStreamButtom));
            }
        }

        public string NameWriteStreamButtom
        {
            get
            {
                return nameWriteStreamButtom;
            }

            set
            {
                nameWriteStreamButtom = value;

                RaisePropertyChanged(nameof(NameWriteStreamButtom));
            }
        }

        public int PercentageBuffer
        {
            get
            {
                return percentageBuffer;
            }

            set
            {
                SetProperty(ref percentageBuffer, value);
            }
        }

        public int PercentageWriteStream
        {
            get
            {
                return percentageWriteStream;
            }
            set
            {
                SetProperty(ref percentageWriteStream, value);
            }
        }

        public ICommand ReadStreamButtomCommand { get { return readStreamButtomCommand; } }

        public ICommand WriteStreamButtomCommand { get { return writeStreamButtomCommand; } }

        private void OnBufferPercentageChanged(long obj)
        {
            PercentageBuffer = (int)obj;
        }

        private void OnWriteStreamPercentageChanged(long obj)
        {
            if (fileSize > 0)
                PercentageWriteStream = (int)(obj * 100 / fileSize);
            else
                PercentageWriteStream = 100;
            
            if(PercentageWriteStream == 100)
            {
                //TODO: Отчистка стримов.
                fileReader.Pause();
                fileWriter.Pause();

                IsEnabled = false;
                MessageBox.Show("Копирование завершено!", "", MessageBoxButton.OK, MessageBoxImage.Information );

                fileReader.Dispose();
                fileWriter.Dispose();
            }
        }

        public void PauseUnpauseReadStream()
        {
            if (stateButtomReadStream != StateButtom.Unpause)
            {
                fileReader.Pause();
                NameReadStreamButtom = "Возобновить";
                stateButtomReadStream = StateButtom.Unpause;
                ColorStreamWriteBuffer = "Red";
            }
            else
            {
                fileReader.Start();
                NameReadStreamButtom = "Приостановить";
                stateButtomReadStream = StateButtom.Pause;
                ColorStreamWriteBuffer = "Green";
            }
        }

        public void PauseUnpauseWriteStream()
        {
            if (stateButtomWriteStream != StateButtom.Unpause)
            {
                fileWriter.Pause();
                NameWriteStreamButtom = "Возобновить";
                stateButtomWriteStream = StateButtom.Unpause;
                ColorStreamWriteFile = "Red";
            }
            else
            {
                fileWriter.Start();
                NameWriteStreamButtom = "Приостановить";
                stateButtomWriteStream = StateButtom.Pause;
                ColorStreamWriteFile = "Green";
            }
        }

        public void InitializeBuffer(string sizeBuffer)
        {
            this.sizeBuffer = Convert.ToInt32(sizeBuffer);

            try
            {
                buffer = new Buffer(this.sizeBuffer, 100);
                buffer.ProgressChanged += OnBufferPercentageChanged;
            }
            catch
            {

            }
        }

        public void InitializeRead(string pathFileRead, string periodRead)
        {
            this.periodRead = Convert.ToInt32(periodRead);

            fileSize = new FileInfo(pathFileRead).Length;

            try
            {

                fileReader = new FileReader(pathFileRead, buffer, this.periodRead);

                fileReader.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        public void InitializeWrite(string pathFileWrite, string periodWrite)
        {
            this.periodWrite = Convert.ToInt32(periodWrite);

            try
            {
                fileWriter = new FileWriter(pathFileWrite, buffer, this.periodWrite);

                fileWriter.Start();
                fileWriter.ProgressChanged += OnWriteStreamPercentageChanged;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }
    }
}