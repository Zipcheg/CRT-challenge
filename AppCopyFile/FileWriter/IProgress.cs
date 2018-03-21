using System;

namespace AppCopyFile
{
    public interface IProgress
    {
        event Action<long> ProgressChanged;
    }
}
