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
        // These are static because the input file is split into seperate strings
        // but we want them to print to one file at the end.
        // They're reset when the printOutput function is called.
        private static int lineCount = 0;
        // Keeps track of accepted and failed Tokens for printing at the end
        private static List<string> AcceptPrint = new List<string>();      
        private static List<string> FailPrint = new List<string>();       
        // Keeps track of number of accepted and failed tokens found.
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
            // line count is incremented at the beginning of each run to keep track of which string we're analyzing
            lineCount++;
            GoldParserTables GPtables = new GoldParserTables();
            int init_state = GPtables.getInitialDFAState();         //convenience for going back to beginning of DFA
            // Keeps track of where we're going (initial value doesn't matter)
            int state_tran = init_state;
            // The staet we begin with
			DFAState state = GPtables.getDFATable()[init_state];
            // Keeps track of the column the token is found at.
            int col_count = 1, col_at = 1;
            string A_form = "|[{0,10}] | {1,12} | {2,27} | {3,-8}|";
            string F_form = "[{0,10}] {1,-5} {2,-3} {3,-3} Unrecognized character: {4,5}";

            TokenStream TStream = new TokenStream();
            // Keeps track of the token as read from the file (not the type)
            string token = "";
            // Symbol index of the token for lookup
			int symbol;

            // Goes through the input character by character
			for (int i = 0; i < input.Length; i++)
			{
                Token test = new Token();
                // Used as a preliminary check if the character is a valid character at all.
                bool check = false;
                // Might not be needed, will check into it in the future.
				foreach (CharSetTableMember c in GPtables.getCharSetTable())
				{
					if (c.getCharSetTableList().Contains(input[i]))
						check = true;
				}
                // Here we just skip the invalid character and add it as an invalid token
				if (!check)
				{
                    FailPrint.Add(string.Format(F_form, failedTokenCount, "At", lineCount + ",", (col_count - 1) + ":", input[i]));
                    AcceptPrint.Add(string.Format(A_form, tokenCount, "Error", 
                        "Unrecognized character: " + input[i], "at " + lineCount + "," + col_count));
                    tokenCount++;
                    failedTokenCount++;
                    TStream.AddToken(new Token(input[i].ToString(),-1,false,col_count));
                    col_count++;
                    continue;
				}

                // Used for keeping count of the transitions from the current DFA state.
				int edgeC = 0;
				foreach (DFAEdge e in state.getEdgeList())
				{
					// Checks if current character is in CharSetTable for current edge.
                    // In which case we make the transition and break from the loop
					if (GPtables.getCharSetTable()[e.getDFAedgeIndex()].getCharSetTableList().Contains(input[i]))
					{
						token += input[i];
						state_tran = e.getDFAedgeTarget();
                        // At this point we don't care about the other edges.
						break;
					}
					else
						edgeC++;
				}

                // In the case none of our edges are valid transitions we arrive here.
				if (edgeC == state.getEdgeCount())      
				{
                    // The i-- is neccessary to backtrack, as the previous character might have ended our valid token.
					i--; 
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
                    if (symbol == -1)
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
					else if (symbol != 2)    //once again ignoring white space
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
