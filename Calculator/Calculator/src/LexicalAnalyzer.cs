using Calculator.XML;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;
using Calculator.Tables;

namespace Calculator
{
    class LexicalAnalyzer
    {
        private static int lineCount = 0;
        private static List<string> AcceptPrint = new List<string>();      //to keep track of Accepted Tokens for printing at the end
        private static List<string> FailPrint = new List<string>();        //to keep track of Failed   Tokens for printing at the end
        private static int tokenCount = 1;
        private static int failedTokenCount = 1;

        public void printOutput(string path)
        {
            string result = @".\Output\debug\" + path + "_LexicalAnalysis.out";
            if (File.Exists(result))
            {
                File.Delete(result);
            }
            Console.Out.Write("Lexical Analyzer Results Printed to: {0}\n", result);
            string[] tempH = { "Token List:", "", string.Format("|{0,12} | {1,12} | {2,27} | {3,8}|", "#", "Type", "Value", "Location")};
            List<string> Header = new List<string>(tempH);
            Header.Add(new String('-', tempH[2].Length));
            string[] E_Header = { "", "Errors:", "" };
            File.WriteAllLines(result, Header);
            File.AppendAllLines(result, AcceptPrint);
            File.AppendAllLines(result, E_Header);
            File.AppendAllLines(result, FailPrint);
            lineCount = 0; tokenCount = 1; failedTokenCount = 1;
            AcceptPrint.Clear();
            FailPrint.Clear();
        }

        public TokenStream getTokenStream(string input)
        {
            lineCount++;
            GoldParserTables GPtables = new GoldParserTables();
            int init_state = GPtables.getInitialDFAState();         //convenience for going back to beginning of DFA
			int state_tran = init_state;                            //keeps track of where we're going (initial value doesn't matter)
			DFAState state = GPtables.getDFATable()[init_state];    //state to begin with
            int col_count = 1, col_at = 1;
            
            string A_form = "|[{0,10}] | {1,12} | {2,27} | {3,-8}|";
            string F_form = "[{0,10}] {1,-5} {2,-3} {3,-3} Unrecognized character: {4,5}";

            TokenStream TStream = new TokenStream();
			string token = "";                                      //keeps track of the token we grab from the file
			int symbol;                                             //index of symbol, for finding name in the table

			for (int i = 0; i < input.Length; i++)           //Goes through the string character by character
			{
                Token test = new Token();
				bool check = false;                     //used for checking if the character is in the char set table at all
				//may be unneccessary/inefficient, might look into it later if time permits
				foreach (CharSetTableMember c in GPtables.getCharSetTable())
				{
					if (c.getCharSetTableList().Contains(input[i]))
						check = true;
				}
				if (!check)                             //if it isn't it's skipped and added to the printout as an error
				{
                    FailPrint.Add(string.Format(F_form, failedTokenCount, "At", lineCount + ",", (col_count - 1) + ":", input[i]));
                    AcceptPrint.Add(string.Format(A_form, tokenCount, "Error", 
                        "Unrecognized character: " + input[i], "at " + lineCount + "," + col_count));
                    tokenCount++;
                    failedTokenCount++;
                    TStream.AddToken(new Token(input[i].ToString(),-1,false,col_count));
                    col_count++;
					continue;   //skips following code and moves on to next element in loop
				}

				int edgeC = 0;
				foreach (DFAEdge e in state.getEdgeList())
				{
					//checks if current character is in CharSetTable for current edge
					if (GPtables.getCharSetTable()[e.getDFAedgeIndex()].getCharSetTableList().Contains(input[i]))
					{
						token += input[i];
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
                        col_at = (col_count - token.Length + 1);
                        string temp = "[{0,5}] {1,-5} {2,-3} {3,-3} Unrecognized character: {4,5}";
                        FailPrint.Add(string.Format(temp, failedTokenCount, "At", lineCount + ",", col_at + ":", input[i]));
                        AcceptPrint.Add(string.Format(A_form, tokenCount, "Error", 
                            "Unrecognized character: " + input[i], "at " + lineCount + "," + col_at));
                        tokenCount++;
                        failedTokenCount++;
                        TStream.AddToken(new Token(token, -1, false, col_at));
						state = GPtables.getDFATable()[init_state];
					}

					else                                        //Accept state
					{
						symbol = state.getAcceptSymbolIndex();
						if (symbol != 2)                        //who cares about white space anyhow
						{
                            col_at = (col_count - token.Length + 1);
                            string token_name = GPtables.getSymbolTable()[symbol].getSymbolTableName();
                            AcceptPrint.Add(string.Format(A_form, tokenCount, token_name, token, "at " + lineCount + "," + col_at));
                            TStream.AddToken(new Token(token, symbol, true, col_at));
                            tokenCount++;
						}
						state_tran = init_state;  //change new state back to initial
					}
					token = "";
				}
				//Transistions to the new DFA state
				state = GPtables.getDFATable()[state_tran];

				if (i == (input.Length - 1))   //if we're at the end of the line we do this as it can't add more characters for doing stuff
				{
					symbol = state.getAcceptSymbolIndex();
					//in retrospect I might be making a bad assumption here, need to check back later
					if (symbol != 2)    //once again ignoring white space
					{
                        col_at = (col_count - token.Length + 1);
                        string token_name = GPtables.getSymbolTable()[symbol].getSymbolTableName();
                        AcceptPrint.Add(string.Format(A_form, tokenCount, token_name, token, "at " + lineCount + "," + col_at));
                        TStream.AddToken(new Token(token,symbol,true,col_at));
                        tokenCount++;
					}
				}
                col_count++;
            }
            TStream.AddToken(new Token("EOF", 0, true, 1));
            return TStream;
		}
    }

}
