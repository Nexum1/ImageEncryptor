using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptor
{
    public class DecryptedFile
    {
        public string Extention { get; set; }
        public byte[] Data { get; set; }

        public override string ToString()
        {
            return Extention;
        }
    }
}
