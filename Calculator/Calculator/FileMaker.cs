using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Calculator
{
    class FileMaker
    {
        private List<string> toPrint;

        public FileMaker()
        {
            toPrint = new List<string>();
        }
        public FileMaker(List<string> tp)
        {
            toPrint = tp;
        }
        public void addToPrint(string addp)
        {
            toPrint.Add(addp);
        }
        public void addToPrint(List<string> addLp)
        {
            toPrint.AddRange(addLp);
        }
        public void print(string path)
        {
            File.WriteAllLines(path, toPrint);
        }
    }
}
