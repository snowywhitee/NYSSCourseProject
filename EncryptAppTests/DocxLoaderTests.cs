using EncryptApp;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EncryptAppTests
{
    public class DocxLoaderTests
    {
        private const string pathTest1 = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\word.docx";
        private const string pathTest2 = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\word2.docx";
        private const string pathTest1res = @"C:\Users\нр\source\repos\EncryptAppTests\TestFiles\word_decrypted.docx";
        [Fact]
        public void DecryptDownloadTest()
        {
            DocxLoader docxLoader = new DocxLoader(pathTest1, Encoding.UTF8);
            docxLoader.Decrypt(new EncryptingMachine());
            docxLoader.Download(pathTest2, true);
            docxLoader = new DocxLoader(pathTest2, Encoding.UTF8);
            DocxLoader docxLoaderRes = new DocxLoader(pathTest1res, Encoding.UTF8);
            Assert.Equal(docxLoaderRes.Print(), docxLoader.Print());
        }
        [Fact]
        public void Overwrite()
        {
            DocxLoader docxLoader = new DocxLoader(pathTest1, Encoding.UTF8);
            try
            {
                docxLoader.Download(pathTest1res, false);
                throw new Exception("not passed");
            }
            catch (Exception ex)
            {
                Assert.Equal("File couldn't be overwritten", ex.Message);
            }
        }
    }
}
