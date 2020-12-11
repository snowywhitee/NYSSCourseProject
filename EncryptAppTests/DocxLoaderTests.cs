using EncryptApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace EncryptAppTests
{
    public class DocxLoaderTests
    {
        private const string pathTest1 = "word.docx";
        private const string pathTest2 = "word2.docx";
        private const string pathTest1res = "word_decrypted.docx";
        private string directory;

        public DocxLoaderTests()
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

        [Fact]
        public void DecryptDownloadTest()
        {
            DocxLoader docxLoader = new DocxLoader(directory + pathTest1, Encoding.UTF8);
            docxLoader.Decrypt(new EncryptingMachine());
            docxLoader.Download(directory + pathTest2, true);
            docxLoader = new DocxLoader(directory + pathTest2, Encoding.UTF8);
            DocxLoader docxLoaderRes = new DocxLoader(directory + pathTest1res, Encoding.UTF8);
            Assert.Equal(docxLoaderRes.Print(), docxLoader.Print());
        }
        [Fact]
        public void Overwrite()
        {
            DocxLoader docxLoader = new DocxLoader(directory + pathTest1, Encoding.UTF8);
            try
            {
                docxLoader.Download(directory + pathTest1res, false);
                throw new Exception("not passed");
            }
            catch (Exception ex)
            {
                Assert.Equal("File couldn't be overwritten", ex.Message);
            }
        }
    }
}
