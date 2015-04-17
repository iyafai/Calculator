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
            if (args.Any())
            {
                doc_lines.AddRange(File.ReadAllLines(args[0]));//@"..\..\Test3.inp"));
            }
            else
            {
                doc_lines.AddRange(File.ReadAllLines(@"..\..\Test3.inp"));
            }
            Dictionary<string, string> varTable = new Dictionary<string, string>();

            XMLParser xmp = new XMLParser();
            xmp.parseAll(); 
            GoldParserTables GPtables = new GoldParserTables();
            GPtables = xmp.getGPBTables();

            LexicalAnalyzer LA = new LexicalAnalyzer();
            TokenStream TStream = new TokenStream();

            //int count = 1, fail_count = 1, line_count = 1, col_count = 1, col_at = 1;
            //These keep track of which token, which error, which line and column we're at

            //Magic Time
            BU_Parser BP = new BU_Parser();
            foreach (string eq in doc_lines)
            {
                TStream = LA.getTokenStream(GPtables,eq);
                AbstractSyntaxTree line1 = BP.ParseStream(GPtables, TStream);
                line1.Calculate(varTable);
            }

            
            
            //varTable.Add("x1", "2");
            
            //Stack<Node> CStack = line1.buildCalcStack(varTable);

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }

}
