using EncryptApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace EncryptAppTests
{
    public class TextLoaderTests
    {
        private const string pathTest1 = "test1.txt";
        private const string pathTest1res = "test1res.txt";
        private const string pathTest2 = "test2.txt";
        private const string pathTest2res = "test2res.txt";
        private const string pathTest3 = "test3.txt";
        private const string pathTest3res = "test3res.txt";
        private const string pathTest4 = "test4.txt";
        private const string pathTest4res = "test4res.txt";
        private const string pathTest5 = "Result_v5.txt";
        private string directory;

        public TextLoaderTests()
        {
            var parent = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            string startDirectory = null;
            if (parent != null)
            {
                var directoryInfo = parent.Parent;
                //string startDirectory = null;
                if (directoryInfo != null)
                {
                    startDirectory = directoryInfo.FullName;
                }
            }
            directory = Path.Combine(startDirectory, @"TestFiles\");
        }

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
        [InlineData(pathTest3, pathTest3res)]
        public void EncryptTest(string passed, string result)
        {
            passed = directory + passed;
            result = directory + result;
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
            passed = directory + passed;
            result = directory + result;
            EncryptingMachine machine = new EncryptingMachine() { Key = "скорпион" };
            TextLoader textLoader = new TextLoader(passed, Encoding.UTF8);
            textLoader.Decrypt(machine);
            string expected = File.ReadAllText(result);

            Assert.Equal(expected, textLoader.Print());
        }
        [Fact]
        public void DecryptTestEncoding()
        {
            string passed = directory + pathTest5;
            string result = directory + pathTest4res;
            EncryptingMachine machine = new EncryptingMachine() { Key = "скорпион" };
            TextLoader textLoader = new TextLoader(passed, CodePagesEncodingProvider.Instance.GetEncoding(1251));
            textLoader.Decrypt(machine);
            string expected = File.ReadAllText(result);
            Assert.Equal(expected, textLoader.Print().Substring(0, expected.Length));
        }
        [Fact]
        public void Overwrite()
        {
            string path = directory + pathTest1;
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
