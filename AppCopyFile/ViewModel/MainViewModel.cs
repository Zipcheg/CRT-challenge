using System;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace AppCopyFile
{
    public class MainViewModel : BindableBase
    {

       
        private readonly DelegateCommand startCommand;
        private readonly DelegateCommand openReadFileCommand;
        private readonly DelegateCommand openWriteFileCommand;

        private string sizeBuffer;
        private string pathFileRead;
        private string pathFileWrite;
        private string periodRead;
        private string periodWrite;


        public MainViewModel()
        {
            PeriodRead = "100";
            PeriodWrite = "200";
            SizeBuffer = "200000";

            startCommand = new DelegateCommand(Start);
            openReadFileCommand = new DelegateCommand(DoOpenReadFileCommand);
            openWriteFileCommand = new DelegateCommand(DoOpenWriteFileCommand);
        }

        public ICommand StartCommand { get { return startCommand; } }

        public ICommand OpenReadFileCommand { get { return openReadFileCommand; } }

        public ICommand OpenWriteFileCommand { get { return openWriteFileCommand; } }

        public bool IsEnabled
        {
            get
            {
                return  !string.IsNullOrEmpty(PathFileRead) 
                    && !string.IsNullOrEmpty(PathFileWrite) 
                    && !string.IsNullOrEmpty(SizeBuffer) 
                    && !string.IsNullOrEmpty(PeriodRead) 
                    && !string.IsNullOrEmpty(PeriodWrite);
            }
        }

        public string PathFileRead
        {
            get
            {
                return pathFileRead;
            }

            set
            {
                pathFileRead = value;
                RaisePropertyChanged(nameof(IsEnabled));
                RaisePropertyChanged(nameof(PathFileRead));
            }
        }

        public string PathFileWrite
        {
            get
            {
                return pathFileWrite;
            }

            set
            {
                pathFileWrite = value;
                RaisePropertyChanged(nameof(IsEnabled));
                RaisePropertyChanged(nameof(PathFileWrite));
            }
        }

        public string SizeBuffer
        {
            get
            {
                if (int.TryParse(sizeBuffer, out int number) && number > 0)
                {
                    return sizeBuffer;
                }
                return sizeBuffer = "";
            }

            set
            {
                sizeBuffer = value;
                RaisePropertyChanged(nameof(IsEnabled));
                RaisePropertyChanged(nameof(SizeBuffer));
            }
        }

        public string PeriodRead
        {
            get
            {
                if (int.TryParse(periodRead, out int number) && number > 0)
                {
                    return periodRead;
                }
                return periodRead = "";
            }

            set
            {
                periodRead = value;
                RaisePropertyChanged(nameof(IsEnabled));
                RaisePropertyChanged(nameof(PeriodRead));
            }
        }

        public string PeriodWrite
        {
            get
            {
                if (int.TryParse(periodWrite, out int number) && number > 0)
                {
                    return periodWrite;
                }
                return periodWrite = "";
            }

            set
            {
                periodWrite = value;
                
                RaisePropertyChanged(nameof(IsEnabled));
                RaisePropertyChanged(nameof(PeriodWrite));

            }
        }

        public void Start()
        {
            try
            {
                using (var file = File.Create(PathFileWrite))
                {
                    file.Seek((Convert.ToInt32(SizeBuffer)), SeekOrigin.Begin);
                }
                Navigation.Navigate("CopyProcessPage", "CopyProcessViewModel", pathFileRead, pathFileWrite, sizeBuffer, periodRead, periodWrite);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DoOpenReadFileCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                FilterIndex = 2,
                RestoreDirectory = true,
                Title = "ReadFile"
            };

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    PathFileRead = openFileDialog.FileName;

                    if (PathFileWrite == PathFileRead)
                    {
                        throw new Exception("Ошибка: Имя файла совпадает с другим именем файла. ");
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                openFileDialog.Reset();
                openFileDialog.ShowDialog();
                PathFileRead = "";
            }
        }

        public void DoOpenWriteFileCommand()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                FilterIndex = 2,
                RestoreDirectory = true,
                CheckFileExists = false,
                Title = "WriteFile"
            };

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    PathFileWrite = openFileDialog.FileName;
                    
                    if (PathFileRead == PathFileWrite )
                    {
                        throw new Exception("Ошибка: Имя файла совпадает с другим именем файла или файл не существует. ");
                    }

                    if (openFileDialog.CheckFileExists)
                    {
                        File.Create(PathFileWrite);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);

                openFileDialog.Reset();
                openFileDialog.ShowDialog();
                openFileDialog.CheckFileExists = false;
                PathFileWrite = "";
            }
        }
    }
}
                
