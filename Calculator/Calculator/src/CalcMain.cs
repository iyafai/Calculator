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

            // In the case of file inputs specified
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
                    string tvalue = "";
                    int maxFormat = 0;

                    // Reads lines out of file, prints error if file not found or inaccessible.
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

                    File.WriteAllText(result, "Results: \n");
                    // Used for formating output
                    // based on the length of the longest equation string up to a max of 80 chars
                    foreach (string eq in doc_lines)
                    {
                        if (eq.Length > maxFormat && eq.Length < 80)
                        {
                            maxFormat = eq.Length;
                        }
                    }

                    foreach (string eq in doc_lines)
                    {
                        TokenStream TStream = LA.getTokenStream(eq);
                        string variableset = TStream.getToken(0).getTokenName();
                        try
                        {
                            // Attempts to parse token stream, calculate and save variable.
                            BP.ParseStream(TStream).Calculate(varTable);
                            varTable.TryGetValue(variableset, out tvalue);
                            calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, tvalue));
                        }
                        // Caught when the parser encounters an error. no variable gets saved.
                        catch (ParseErrorException e)
                        {
                            calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, "invalid"));
                            calculatorOutput.Add(e.Message + "\n");
                        }
                        // Caught when the calculator hits an error (div/0, etc)
                        catch (CalculationErrorException c)
                        {
                            try
                            {
                                varTable.Add(TStream.getToken(0).getTokenName(), "0");
                                varTable.TryGetValue(variableset, out tvalue);
                                calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, tvalue));
                            }
                            // For edge case of calculation error & overwrite attempt
                            catch (ArgumentException)
                            {
                                varTable.TryGetValue(variableset, out tvalue);
                                calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, tvalue));
                                calculatorOutput.Add("**Warning: Variable already defined. Future calculations will use new value");
                            }
                            calculatorOutput.Add(c.Message + "\n");
                        }
                        // Caught when calculator tries overwriting a previously saved variable
                        catch (ArgumentException ae)
                        {
                            varTable.TryGetValue(variableset, out tvalue);
                            calculatorOutput.Add(string.Format("{0,-" + maxFormat + "} => {1}", eq, tvalue));
                            calculatorOutput.Add(ae.Message + "\n");
                        }
                        // Just in case for debugging purposes
                        catch (Exception ex)
                        {
                            Console.Out.Write(ex.Message);
                        }
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

            // Allows for equation solving in the console, in the case of no inputs
            else
            {
                // Stores variable values
                Dictionary<string, string> varTable = new Dictionary<string, string>();
                BU_Parser BP = new BU_Parser();
                LexicalAnalyzer LA = new LexicalAnalyzer();
                List<string> calculatorOutput = new List<string>();
                Console.Out.Write("Input Equation to solve\n Equations must be of form: variable = Expression;\n");
                Console.Out.Write(" eg. x3 = 4*(x1-5);\n type 'quit' to end\n");
                string eq = "";
                string tvalue = "";

                while (true)
                {
                    eq = Console.In.ReadLine();
                    // Typing quit ends equation solver
                    if (eq.Equals("quit", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    TokenStream TStream = LA.getTokenStream(eq);
                    string variableset = TStream.getToken(0).getTokenName();
                    // The following code works the same as above, only it prints to the console instead of a file
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
                    }
                    catch (CalculationErrorException c)
                    {
                        try
                        {
                            varTable.Add(TStream.getToken(0).getTokenName(), "0");
                            varTable.TryGetValue(variableset, out tvalue);
                            Console.Out.Write(String.Format("{0,10} = {1}\n", variableset, tvalue));
                        }
                        catch (ArgumentException)
                        {
                            varTable.TryGetValue(variableset, out tvalue);
                            Console.Out.Write(String.Format("{0,10} = {1}\n", variableset, tvalue));
                            Console.Out.Write("**Warning: Variable already defined. Future calculations will use new value\n");
                        }
                        Console.Out.Write(c.Message + "\n");
                    }
                    catch (ArgumentException ae)
                    {
                        varTable.TryGetValue(variableset, out tvalue);
                        Console.Out.Write(String.Format("{0,10} = {1}\n", variableset, tvalue));
                        Console.Out.Write(ae.Message + "\n");
                    }
                    catch (Exception ex)
                    {
                        Console.Out.Write(ex.Message + "\n");
                    }
                }
            }

            Console.WriteLine("Press any key to close...");
            Console.Read();
        }
    }

}
