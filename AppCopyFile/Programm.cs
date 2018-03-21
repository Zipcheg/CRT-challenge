using System;
using System.Windows;
using System.ComponentModel;
using System.Threading;

namespace AppCopyFile
{
    class Programm
    {
        [STAThread]
        static void Main()
        {
            App app = new App();
            MainWindow main = new MainWindow();
            try
            {
                main.Closing += Main_Closing;
                using (var token = new CancellationTokenSource())
                {
                    Navigation.NavigationService = main.MainFrame.NavigationService;

                    Navigation.Navigate("ConfigPage", "MainViewModel");

                    app.Run(main);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private static void Main_Closing(object sender, CancelEventArgs e)
        {
            //TODO: Правильно закрыть приложение.
            Application.Current.Shutdown();
        }
    }
}
