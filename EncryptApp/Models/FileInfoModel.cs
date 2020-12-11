using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Net.WebRequestMethods;

namespace EncryptApp.Models
{
    public class FileInfoModel
    {
        [Required(ErrorMessage = "You must specify the file path!")]
        public string FilePath { get; set; }
        public string Key { get; set; }
        public string Encoding { get; set; }
        [Required]
        public bool Encrypt { get; set; }

        public Encoding GetEncoding()
        {
            if (Encoding == null) Encoding = "UTF-8";
            switch (Encoding)
            {
                case "UTF-8":
                    return System.Text.Encoding.UTF8;
                case "ANSI":
                    return CodePagesEncodingProvider.Instance.GetEncoding(1251);
                case "ASCII":
                    return System.Text.Encoding.ASCII;
                default:
                    return System.Text.Encoding.UTF8;
            }
        }
    }
}
