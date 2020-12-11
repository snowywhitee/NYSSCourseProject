using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EncryptApp.Models
{
    public class DownloadFileInfoModel
    {
        [Required(ErrorMessage = "You must specify the file path!")]
        public string FilePath { get; set; }
        public bool Overwrite { get; set; }
    }
}

