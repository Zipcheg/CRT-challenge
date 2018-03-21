using System.IO;

namespace AppCopyFile
{
    public interface IBuffer
    {
        bool TryCopyTo(FileStream fileStream, out int readed);
        bool TryCopyFrom(FileStream fileStream, out int written);
    }
}
