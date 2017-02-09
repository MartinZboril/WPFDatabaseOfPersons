using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabazeOsob
{
    class FileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            return System.IO.Path.Combine("", filename);
        }
    }
}
