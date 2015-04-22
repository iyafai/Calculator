using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.XML;
using System.IO;
using System.Xml;
using System.Reflection;

[assembly: AssemblyVersion("1.0.0")]

namespace Calculator
{
    class CalcMain
    {
        public static string userPathLoc = Environment.CurrentDirectory;
        public static string outputPath = userPathLoc + @"\Output\";
        public static void Main(string[] args)
        {
            string ast = "************************************";
            XMLParser xmp = new XMLParser();
            xmp.parseAll();
            GoldParserTables GPtables = xmp.getGPBTables();     //Symbol, Rule, Parse, DFA and CharSet Tables
            string p1 = Environment.CurrentDirectory;

            //string result = "Calculator.out";
            //pulls each line from the file and adds to its own string in the List
            System.IO.Directory.CreateDirectory(outputPath);
            System.IO.Directory.CreateDirectory(p1+@"\Output\debug");

            if (args.Any())
            {
                foreach (string test in args)
                {
                    Dictionary<string, string> varTable = new Dictionary<string, string>();     //Stores variable values
                    List<string> doc_lines = new List<string>();        //Contains each Equation from file as a seperate string
                    BU_Parser BP = new BU_Parser();
                    LexicalAnalyzer LA = new LexicalAnalyzer();
                    List<string> calculatorOutput = new List<string>();
                    string fname = Path.GetFileNameWithoutExtension(test);
                    string result = outputPath + fname + ".out";
                    int maxFormat = 0;
                    string tvalue = "";

                    try
                    {
                        doc_lines.AddRange(File.ReadAllLines(test));
                    }
                    catch (Exception ex)
                    {
                        if (ex is FileNotFoundException || ex is FieldAccessException)
                        {
                            System.Console.Out.Write("{0}\n", ex.Message);
                            break;
                        }
                    }

                    File.WriteAllText(result, "Variables: \n");
                    foreach (string eq in doc_lines)
                    {
                        if (eq.Length > maxFormat && eq.Length < 80)
                        {
                            maxFormat = eq.Length;
                        }
                    }

                    int linecount = 0;
                    foreach (string eq in doc_lines)
                    {
                        TokenStream TStream = LA.getTokenStream(eq);
                        string variableset = TStream.getToken(0).getTokenName();
                        try
                        {
                            BP.ParseStream(TStream).Calculate(varTable);
                            varTable.TryGetValue(variableset, out tvalue);
                            calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, tvalue));
                        }
                        catch (ParseErrorException e)
                        {
                            calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, "invalid"));
                            calculatorOutput.Add(e.Message + "\n");
                            linecount--;
                        }
                        catch (CalculationErrorException c)
                        {
                            try
                            {
                                if (varTable.ContainsKey(variableset))
                                {
                                    varTable.TryGetValue(variableset, out tvalue);
                                }
                            }
                            catch (ArgumentNullException)
                            {
                                varTable.Add(variableset, "0");
                                varTable.TryGetValue(variableset, out tvalue);
                            }
                            calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, tvalue));
                            calculatorOutput.Add(c.Message + "\n");
                            linecount--;
                        }
                        catch (Exception ex)
                        {
                            Console.Out.Write(ex.Message);
                        }
                        linecount++;
                    }

                    LA.printOutput(fname);
                    BP.printOutput(fname);
                    calculatorOutput.Add(ast);
                    calculatorOutput.Add("Symbol Table");
                    calculatorOutput.Add(ast);
                    foreach (KeyValuePair<string, string> temp in varTable)
                    {
                        calculatorOutput.Add(string.Format("{0,-5}: {1}", temp.Key, temp.Value));
                    }
                    File.AppendAllLines(result, calculatorOutput);
                    Console.Out.Write("Finished Calculating. Output Saved to: {0}\n\n", result);
                    //System.Diagnostics.Process.Start(result);
                }
            }
            else
            {
                Dictionary<string, string> varTable = new Dictionary<string, string>();     //Stores variable values
                BU_Parser BP = new BU_Parser();
                LexicalAnalyzer LA = new LexicalAnalyzer();
                List<string> calculatorOutput = new List<string>();
                Console.Out.Write("Input Equation to solve\n Equations must be of form: variable = Expression;\n");
                Console.Out.Write(" eg. x3 = 4*(x1-5);\n type 'quit' to end\n");
                string eq = "";
                string tvalue = "";
                int linecount = 0;

                while (true)
                {
                    eq = Console.In.ReadLine();
                    if (eq.Equals("quit", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    TokenStream TStream = LA.getTokenStream(eq);
                    string variableset = TStream.getToken(0).getTokenName();
                    try
                    {
                        BP.ParseStream(TStream).Calculate(varTable);
                        varTable.TryGetValue(variableset, out tvalue);
                        Console.Out.Write(String.Format("{0,10} = {1}\n", variableset, tvalue));
                    }
                    catch (ParseErrorException e)
                    {
                        Console.Out.Write(String.Format("{0,10} => {1}\n", eq, "invalid"));
                        Console.Out.Write(e.Message + "\n");
                        linecount--;
                    }
                    catch (CalculationErrorException c)
                    {
                        try
                        {
                            if (varTable.ContainsKey(TStream.getToken(0).getTokenName()))
                            {
                                varTable.TryGetValue(variableset, out tvalue);
                            }
                        }
                        catch (ArgumentNullException)
                        {
                            varTable.Add(TStream.getToken(0).getTokenName(), "0");
                            varTable.TryGetValue(variableset, out tvalue);
                        }
                        Console.Out.Write(String.Format("{0,10} = {1}\n", variableset, tvalue));
                        Console.Out.Write(c.Message + "\n");
                        linecount--;
                    }
                    catch (Exception ex)
                    {
                        Console.Out.Write(ex.Message+"\n");
                    }
                    linecount++;
                }
            }

            Console.WriteLine("Press any key to close...");
            Console.Read();
        }
    }

}
