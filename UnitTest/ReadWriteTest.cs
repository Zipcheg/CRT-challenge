using AppCopyFile;
using NUnit.Framework;
using System.Threading;

namespace UnitTest
{
    [TestFixture]
    public class ReadWriteTest
    {
        [Test]
        public void MyTest()
        {
            IBuffer buffer = new Buffer(200000, 100);
            var reader = new FileReader(@"C:\Temp\vvv.txt", buffer, 10);
            var writer = new FileWriter(@"C:\Temp\ggg.txt", buffer, 10);
            reader.Start();
            writer.Start();

            Thread.Sleep(10000);
                
        }
    }
}
