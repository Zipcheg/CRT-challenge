namespace AppCopyFile
{
    public interface IInitializable
    {
        void InitializeBuffer(string sizeBuffer);

        void InitializeRead(string pathFileRead, string periodRead);

        void InitializeWrite(string pathFileWrite, string periodWrite);
    }
}