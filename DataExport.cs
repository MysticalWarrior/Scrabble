using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Scrabble
{
    class DataExport
    {
        private StreamWriter exporter;
        public DataExport(string path)
        {
            exporter = new StreamWriter(path);
        }
        public void write(string s)
        {
            exporter.Write(s);
        }
        public void writeLine(string s)
        {
            exporter.Write(s + '\r');
        }
    }
}
