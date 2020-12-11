using EncryptApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace EncryptAppTests
{
    public class TextLoaderTests
    {
        private const string pathTest1 = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\test1.txt";
        private const string pathTest1res = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\test1res.txt";
        private const string pathTest2 = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\test2.txt";
        private const string pathTest2res = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\test2res.txt";
        private const string pathTest3 = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\test3.txt";
        private const string pathTest3res = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\test3res.txt";
        private const string pathTest4 = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\test4.txt";
        private const string pathTest4res = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\test4res.txt";
        private const string pathTest5 = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\Result_v5.txt";


        [Theory]
        [InlineData(null, "File path can't be null or empty")]
        [InlineData("", "File path can't be null or empty")]
        [InlineData(@"F:\file\file\file\file.txt", @"File F:\file\file\file\file.txt not found")]
        public void InvalidPath(string path, string expectedMsg)
        {
            try
            {
                TextLoader textLoader = new TextLoader(path, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Assert.Equal(expectedMsg, ex.Message);
            }
        }

        [Theory]
        [InlineData(pathTest1, pathTest1res)]
        //[InlineData(pathTest3, pathTest3res)]
        public void EncryptTest(string passed, string result)
        {
            EncryptingMachine machine = new EncryptingMachine() { Key = "скорпион" };
            TextLoader textLoader = new TextLoader(passed, Encoding.UTF8);
            textLoader.Encrypt(machine);
            string expected = File.ReadAllText(result);
            
            Assert.Equal(expected, textLoader.Print());
        }

        [Theory]
        [InlineData(pathTest2, pathTest2res)]
        [InlineData(pathTest4, pathTest4res)]
        public void DecryptTest(string passed, string result)
        {
            EncryptingMachine machine = new EncryptingMachine() { Key = "скорпион" };
            TextLoader textLoader = new TextLoader(passed, Encoding.UTF8);
            textLoader.Decrypt(machine);
            string expected = File.ReadAllText(result);

            Assert.Equal(expected, textLoader.Print());
        }
        [Fact]
        public void DecryptTestEncoding()
        {
            EncryptingMachine machine = new EncryptingMachine() { Key = "скорпион" };
            TextLoader textLoader = new TextLoader(pathTest5, CodePagesEncodingProvider.Instance.GetEncoding(1251));
            textLoader.Decrypt(machine);
            string expected = File.ReadAllText(pathTest4res);
            //make another file//
            Assert.Equal(expected, textLoader.Print().Substring(0, expected.Length));
        }
        [Fact]
        public void Overwrite()
        {
            string path = pathTest1;
            TextLoader textLoader = new TextLoader(path, Encoding.UTF8);
            try
            {
                textLoader.Download(path, false);
                throw new Exception("not passed");
            }
            catch (Exception ex)
            {
                Assert.Equal("File couldn't be overwritten", ex.Message);
            }
        }
    }
}
