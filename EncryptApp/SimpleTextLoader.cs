using System;
using System.IO;
using System.Text;

namespace EncryptApp
{
    public class SimpleTextLoader
    {
        protected char[] text;
        protected Encoding encoding;
        public SimpleTextLoader(string text, Encoding encoding)
        {
            this.text = text.ToCharArray();
            if (encoding == null) encoding = Encoding.UTF8;
            this.encoding = encoding;
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
            return Print().Replace(Environment.NewLine, "<br />");
        }
        public virtual void Download(string newPath, bool overwrite)
        {
            HandleOverwriting(newPath, overwrite);
            using (FileStream fs = File.Create(newPath))
            {
                using (StreamWriter sw = new StreamWriter(fs, encoding))
                {
                    sw.Write(text);
                }
            }
        }

        //Helper methods
        protected void HandleOverwriting(string path, bool overwrite)
        {
            if (File.Exists(path))
            {
                if (overwrite)
                {
                    File.Delete(path);
                }
                else
                {
                    throw new TextLoaderException("File couldn't be overwritten");
                }
            }
        }
    }
}
