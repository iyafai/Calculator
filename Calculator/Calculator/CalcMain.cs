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
            string ast = "************************************";
            XMLParser xmp = new XMLParser();
            xmp.parseAll();
            GoldParserTables GPtables = xmp.getGPBTables();     //Symbol, Rule, Parse, DFA and CharSet Tables
            

            //string result = "Calculator.out";
            //pulls each line from the file and adds to its own string in the List
            System.IO.Directory.CreateDirectory(@".\Output");
            System.IO.Directory.CreateDirectory(@".\Output\extra");

            foreach (string test in args)
            {
                Dictionary<string, string> varTable = new Dictionary<string, string>();     //Stores variable values
                List<string> doc_lines = new List<string>();        //Contains each Equation from file as a seperate string
                List<string> printout_FH = new List<string>();
                List<string> printout_SH = new List<string>();
                string fname = Path.GetFileNameWithoutExtension(test);
                string result = fname+".out";
                string printline1 = "";
                int maxFormat = 0;
                List<string> Footer = new List<string>();
                Footer.Add(ast);
                Footer.Add("Symbol Table");
                Footer.Add(ast);

                try
                {
                    doc_lines.AddRange(File.ReadAllLines(test));//@"..\..\Test3.inp"));
                }
                catch(Exception ex)
                {
                    if (ex is FileNotFoundException || ex is FieldAccessException)
                    {
                        System.Console.Out.Write("{0}\n", ex.Message);
                    }
                }
                BU_Parser BP = new BU_Parser();
                BP.createOutput(fname);
                LexicalAnalyzer LA = new LexicalAnalyzer();
                LA.createOutput(fname);
                foreach (string eq in doc_lines)
                {
                    if (eq.Length > maxFormat)
                    {
                        maxFormat = eq.Length;
                    }
                    printline1 = eq;
                    printout_FH.Add(eq);
                    
                    TokenStream TStream = LA.getTokenStream(GPtables, eq);
                    //AbstractSyntaxTree line1 = BP.ParseStream(GPtables, TStream);
                    try
                    {
                        BP.ParseStream(GPtables, TStream).Calculate(varTable);
                    }
                    catch (ParseErrorException e)
                    {
                        Console.Out.Write(e.Message);
                    }
                }

                File.WriteAllText(result, "Variables: \n");
                int linecount=0;
                foreach(KeyValuePair<string,string> temp in varTable)
                {
                    string printline = string.Format("{0,-"+maxFormat+"} => {1}", printout_FH.ElementAt(linecount), temp.Value);
                    Footer.Add(string.Format("{0,-5}: {1}", temp.Key, temp.Value));
                    printout_SH.Add(printline);//printout_FH.ElementAt(i)+" => "+printline);
                    linecount++;
                }
                File.AppendAllLines(result, printout_SH);
                File.AppendAllLines(result,Footer);
                Console.Out.Write("Results Printed to: {0}\n", @".\"+result);
                //System.Diagnostics.Process.Start(result);
            }

            Console.WriteLine("Press any key to close...");
            Console.Read();
        }
    }

}
