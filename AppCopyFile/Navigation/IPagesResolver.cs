using System.Windows.Controls;

namespace AppCopyFile
{
    public interface IPagesResolver
    {
        Page GetPageInstance(string pageName);
    }

}
