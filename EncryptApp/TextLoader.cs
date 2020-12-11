using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace EncryptApp
{
    public class TextLoader : SimpleTextLoader
    {
        protected string path;

        public TextLoader(string path, Encoding encoding) : base("", encoding)
        {
            //Check for errors
            if (path == null || path == "") throw new TextLoaderException("File path can't be null or empty");
            if (!File.Exists(path)) throw new TextLoaderException($"File {path} not found");
            
            //Initialize
            this.path = path;
            Load();
        }

        //Methods
        public virtual void Load()
        {
            text = File.ReadAllText(path, encoding).ToCharArray();
        }
    }
    public class TextLoaderException : Exception
    {
        public TextLoaderException(string msg) : base(msg)
        {

        }
    }
}
