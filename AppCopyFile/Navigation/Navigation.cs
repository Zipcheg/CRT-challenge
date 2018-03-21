using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AppCopyFile
{
    partial class Navigation
    {
        private NavigationService navigationService;
        private readonly IPagesResolver pagesResolver;
        private IViewModelsResolver vmsResolver;

        public static NavigationService NavigationService
        {
            get
            {
                return Instance.navigationService;
            }
            set
            {
                if (Instance.navigationService != null)
                {
                    Instance.navigationService.Navigated -= Instance.NavigationService_Navigated;
                }

                Instance.navigationService = value;
                Instance.navigationService.Navigated += Instance.NavigationService_Navigated;
            }
        }

        public static void Navigate(string pageUri, string contextUri)
        {
            if (Instance.navigationService == null || pageUri == null)
            {
                return;
            }

            var page = Instance.pagesResolver.GetPageInstance(pageUri);
            var context = Instance.vmsResolver.GetViewModelInstance(contextUri);
            Instance.navigationService.Navigate(page, context);
        }

        public static void Navigate(string pageUri, string contextUri, string pathFileRead, string pathFileWrite, string sizeBuffer, string periodRead, string periodWrite)
        {
            if (Instance.navigationService == null || pageUri == null)
            {
                return;
            }

            var page = Instance.pagesResolver.GetPageInstance(pageUri);
            var context = Instance.vmsResolver.GetViewModelInstance(contextUri);

            if (context is IInitializable)
            {
                (context as IInitializable).InitializeBuffer(sizeBuffer);
                (context as IInitializable).InitializeRead(pathFileRead, periodRead);
                (context as IInitializable).InitializeWrite(pathFileWrite, periodWrite);

                Instance.navigationService.Navigate(page, context);
            }
            else
            {
                throw new ArgumentException("Wrong context parameter");
            }
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            var page = e.Content as Page;

            if (page == null)
            {
                return;
            }

            page.DataContext = e.ExtraData;
        }
        

private static volatile Navigation _instance;
        private static readonly object Sync = new Object();

        private Navigation()
        {
            pagesResolver = new PagesResolver();
            vmsResolver = new ViewModelsResolver();
        }

        private static Navigation Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Sync)
                    {
                        if (_instance == null)
                            _instance = new Navigation();
                    }
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}
