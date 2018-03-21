using System.Collections.Generic;
using System.Windows.Controls;

namespace AppCopyFile
{
    class PagesResolver : IPagesResolver
    {
        private readonly Dictionary<string, Page> pagesResolver = new Dictionary<string, Page>();

        public PagesResolver()
        {
            pagesResolver.Add("CopyProcessPage", new CopyProcessPage()); 
            pagesResolver.Add("ConfigPage", new ConfigPage());
        }

        public Page GetPageInstance(string pageName)
        {
            return pagesResolver[pageName];
        }
    }
}
