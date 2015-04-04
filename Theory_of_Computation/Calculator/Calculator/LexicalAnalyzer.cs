//Imran Yafai
//Theory of Computation - Lexical Analyzer
using Calculator.XML;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Calculator
{
    class LexicalAnalyzer
    {
        public static void Main(string[] args)
        {
            string result = "LAresult-T2.txt";
            string token_name = "";
            List<string> AcceptPrint = new List<string>();      //to keep track of Accepted Tokens for printing at the end
            List<string> FailPrint = new List<string>();        //to keep track of Failed   Tokens for printing at the end
            string A_form = "[{0,5}] {1,10} {2,10} {3,-8}\n";
            string F_form = "[{0,5}] {1,-5} {2,-3} {3,-3} Unrecognized character: {4,5}\n";

            XMLParser xmp = new XMLParser();
            xmp.parseAll();
            GoldParserTables GPtables = new GoldParserTables();
            GPtables = xmp.getGPBTables();

            //pulls each line from the file and adds to its own string in the List
            List<string> doc_lines = new List<string>();
            doc_lines.AddRange(File.ReadAllLines(@"..\..\Test2.inp")); 

            int count = 1, fail_count = 1, line_count = 1, col_count = 1, col_at = 1;
            //These keep track of which token, which error, which line and column we're at

            //Magic Time
            foreach (string eq in doc_lines)
            {
                int init_state = GPtables.getInitialDFAState();         //convenience for going back to beginning of DFA
                int state_tran = init_state;                            //keeps track of where we're going (initial value doesn't matter)
                DFAState state = GPtables.getDFATable()[init_state];    //state to begin with
                string token = "";                                      //keeps track of the token we grab from the file
                int symbol;                                             //index of symbol, for finding name in the table

                for (int i = 0; i < eq.Length; i++)           //Goes through the string character by character
                {
                    bool check = false;                     //used for checking if the character is in the char set table at all
                                                            //may be unneccessary/inefficient, might look into it later if time permits
                    foreach (CharSetTableMember c in GPtables.getCharSetTable())
                    {
                        if (c.getCharSetTableList().Contains(eq[i]))
                            check = true;
                    }
                    if (!check)                             //if it isn't it's skipped and added to the printout as an error
                    {
                        FailPrint.Add(string.Format(F_form, fail_count, "At", line_count + ",", (col_count-1) + ":", eq[i]));
                        AcceptPrint.Add(string.Format(A_form, count, "Error", "Unrecognized character: " + eq[i], "at " + line_count + "," + col_count));
                        count++;
                        fail_count++;
                        col_count++;
                        continue;   //skips following code and moves on to next element in loop
                    }

                    int edgeC = 0;
                    foreach (DFAEdge e in state.getEdgeList())
                    {
                        //checks if current character is in CharSetTable for current edge
                        if (GPtables.getCharSetTable()[e.getDFAedgeIndex()].getCharSetTableList().Contains(eq[i]))
                        {
                            token += eq[i];
                            state_tran = e.getDFAedgeTarget();   //new target for DFA
                            break;  //if found loop breaks as the other edges are unneccessary by this point.
                        }
                        else
                            edgeC++;    //next edge, rinse repeat
                    }

                    if (edgeC == state.getEdgeCount())      //if we've exhausted all edges we arrive here 
                    {
                        i--;            //Since we didn't add the current character (due to no transitions) we backtrack a bit in here. 
                        col_count--;

                        if (state.getAcceptSymbolIndex() == -1)     //No acceptance, token is added to error printout
                        {
                            col_at = (col_count - token.Length+1);
                            string temp = "[{0,5}] {1,-5} {2,-3} {3,-3} Unrecognized character: {4,5}\n";
                            FailPrint.Add(string.Format(temp, fail_count, "At", line_count + ",", col_at + ":", eq[i]));
                            AcceptPrint.Add(string.Format(A_form, count, "Error", "Unrecognized character: " + eq[i], "at " + line_count + "," + col_at));
                            count++;
                            fail_count++;
                            state = GPtables.getDFATable()[init_state];
                        }

                        else                                        //Accept state
                        {
                            symbol = state.getAcceptSymbolIndex();
                            if (symbol != 2)                        //who cares about white space anyhow
                            {
                                col_at = (col_count - token.Length+1);
                                token_name = GPtables.getSymbolTable()[symbol].getSymbolTableName();
                                AcceptPrint.Add(string.Format(A_form, count, token_name, token, "at " + line_count + "," + col_at));
                                count++;
                            }
                            state_tran = init_state;  //change new state back to initial
                        }
                        token = "";
                    }
                    //Transistions to the new DFA state
                    state = GPtables.getDFATable()[state_tran];

                    if (i == (eq.Length - 1))   //if we're at the end of the line we do this as it can't add more characters for doing stuff
                    {
                        symbol = state.getAcceptSymbolIndex();
                        //in retrospect I might be making a bad assumption here, need to check back later
                        if (symbol != 2)    //once again ignoring white space
                        {
                            col_at = (col_count - token.Length+1);
                            token_name = GPtables.getSymbolTable()[symbol].getSymbolTableName();
                            AcceptPrint.Add(string.Format(A_form, count, token_name, token, "at " + line_count + "," + col_at));
                            count++;
                        }
                    }
                    col_count++;
                }
                col_count = 1;  //resets column count
                col_at = 1;     //
                line_count++;
            }
            //printing out for EOF as it doesn't seem to get read elsewhere, not sure if it's excluded by the way 
            //I'm pulling it from the file or if I'm doing something wrong elsewhere, at any rate the file is getting 
            //read through and we know the EOF is going to be there at the end
            token_name = GPtables.getSymbolTable()[0].getSymbolTableName();
            string symbol_name = GPtables.getSymbolTable()[0].getSymbolTableName();
            AcceptPrint.Add(string.Format(A_form, count, token_name, symbol_name, "at " + line_count + "," + col_at));
            string[] Header = { "Token List:\n", "\n", string.Format("{0,7} {1,10} {2,10} {3,8}\n", "#", "Type", "Value", "Location") };
            string[] E_Header = { "\n", "Errors:\n", "\n" };
            
            //print everything out all nice and neat
            File.WriteAllLines(result, Header);
            File.AppendAllLines(result, AcceptPrint);
            File.AppendAllLines(result, E_Header);
            File.AppendAllLines(result, FailPrint);
        }
    }
}
