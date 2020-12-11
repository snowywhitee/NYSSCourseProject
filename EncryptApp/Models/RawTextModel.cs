using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptApp.Models
{
    public class RawTextModel
    {
        public string Key { get; set; }
        [Required]
        public string Text { get; set; }
        public bool Encrypt { get; set; }
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
