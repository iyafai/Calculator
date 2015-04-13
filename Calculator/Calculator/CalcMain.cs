using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.XML;
using System.IO;

namespace Calculator
{
    class CalcMain
    {
        public static void Main(string[] args)
        {
            //string result = "Calculator.out";
            //pulls each line from the file and adds to its own string in the List
            List<string> doc_lines = new List<string>();
            doc_lines.AddRange(File.ReadAllLines(@"..\..\Test3.inp"));

            XMLParser xmp = new XMLParser();
            xmp.parseAll(); 
            GoldParserTables GPtables = new GoldParserTables();
            GPtables = xmp.getGPBTables();

            LexicalAnalyzer LA = new LexicalAnalyzer();
            TokenStream TStream = new TokenStream();

            //int count = 1, fail_count = 1, line_count = 1, col_count = 1, col_at = 1;
            //These keep track of which token, which error, which line and column we're at

            //Magic Time
            foreach (string eq in doc_lines)
            {
                TStream = LA.getTokenStream(GPtables,eq);
            }

            BU_Parser BP = new BU_Parser();
            AbstractSyntaxTree line1 = BP.ParseStream(GPtables, TStream);
            
        }
    }

}
