using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace EncryptApp
{
    public class TextLoader
    {
        protected string path;
        protected char[] text;
        private Encoding encoding;
        public TextLoader(string path, Encoding encoding)
        {
            if (path == null || path == "") throw new Exception("File path can't be null or empty");
            if (!File.Exists(path)) throw new Exception($"File {path} not found");
            this.encoding = encoding;
            if (encoding == null) this.encoding = Encoding.UTF8;
            this.path = path;
            Load();
        }
        public virtual void Load()
        {
            text = File.ReadAllText(path, encoding).ToCharArray();
        }
        public void Encrypt(EncryptingMachine machine)
        {
            text = machine.Encrypt(text);
        }
        public void Decrypt(EncryptingMachine machine)
        {
            text = machine.Decrypt(text);
        }
        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                sb.Append(text[i]);
            }
            return sb.ToString();
        }
        public string HtmlPrint()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                sb.Append(text[i]);
            }
            return sb.ToString().Replace(Environment.NewLine, "<br />");
        }
        public virtual void Download(string newPath, bool overwrite)
        {
            if (File.Exists(newPath))
            {
                if (overwrite)
                {
                    File.Delete(newPath);
                }
                else
                {
                    throw new TextLoaderException("File couldn't be overwritten");
                }
            }
            using (FileStream fs = File.Create(newPath))
            {
                using (StreamWriter sw = new StreamWriter(fs, encoding))
                {
                    sw.Write(text);
                }
            }
        }
    }
    public class TextLoaderException : Exception
    {
        public TextLoaderException(string msg) : base(msg)
        {

        }
    }
}
