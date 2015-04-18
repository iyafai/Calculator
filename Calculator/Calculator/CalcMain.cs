﻿using System;
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
            System.IO.Directory.CreateDirectory(@".\Output\debug");

            foreach (string test in args)
            {
                Dictionary<string, string> varTable = new Dictionary<string, string>();     //Stores variable values
                List<string> doc_lines = new List<string>();        //Contains each Equation from file as a seperate string
                BU_Parser BP = new BU_Parser();
                LexicalAnalyzer LA = new LexicalAnalyzer();
                List<string> calculatorOutput = new List<string>();
                string fname = Path.GetFileNameWithoutExtension(test);
                string result = @".\Output\"+fname+".out";
                int maxFormat = 0;

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

                File.WriteAllText(result, "Variables: \n");
                foreach (string eq in doc_lines)
                {
                    if (eq.Length > maxFormat)
                    {
                        maxFormat = eq.Length;
                    }
                }

                int linecount = 0;
                foreach (string eq in doc_lines)
                {
                    TokenStream TStream = LA.getTokenStream(GPtables, eq);
                    try
                    {
                        BP.ParseStream(GPtables, TStream).Calculate(varTable);
                        string tvalue = varTable.ElementAt(linecount).Value;
                        calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, tvalue));
                    }
                    catch (ParseErrorException e)
                    {
                        calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, "invalid"));
                        calculatorOutput.Add(e.Message);
                        linecount--;
                    }
                    linecount++;
                }

                LA.printOutput(fname);
                BP.printOutput(fname);
                calculatorOutput.Add(ast);
                calculatorOutput.Add("Symbol Table");
                calculatorOutput.Add(ast);
                foreach(KeyValuePair<string,string> temp in varTable)
                {
                    calculatorOutput.Add(string.Format("{0,-5}: {1}", temp.Key, temp.Value));
                }
                File.AppendAllLines(result, calculatorOutput);
                Console.Out.Write("Results Printed to: {0}\n", result);
                //System.Diagnostics.Process.Start(result);
            }

            Console.WriteLine("Press any key to close...");
            Console.Read();
        }
    }

}
