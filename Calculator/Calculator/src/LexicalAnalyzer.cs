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
            string[] tempH = { "Token List:", "", string.Format("|{0,12} | {1,12} | {2,27} | {3,8}|", " ", "Type", "Value", "Location")};
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
            int initialstate = GPtables.getInitialDFAState();         //convenience for going back to beginning of DFA
            // Keeps track of where we're going (initial value doesn't matter)
            int newstate = initialstate;
            // The state we begin with
			DFAState state = GPtables.getDFATable()[initialstate];
            // Keeps track of the column the token is found at.
            int atCol = 1;
            string A_form = "|#{0,10}  | {1,12} | {2,27} | {3,-8}|";
            string F_form = "[{0,10}] {1,-5} {2,-3} {3,-3} Unrecognized character: {4,5}";

            TokenStream TStream = new TokenStream();
            // Keeps track of the token as read from the file (not the type)
            string token = "";
            // Symbol index of the token for lookup
			int symbol;

            // Goes through the input character by character
			for (int i = 0; i < input.Length; i++)
			{
                bool transition = true;
                bool validCharacter = false;
                int edgeCount = 0;
                atCol = i + 1;

                // A bit cumbersome, but I haven't thought of a better way to check this yet.
                foreach (CharSetTableMember c in GPtables.getCharSetTable())
                {
                    if (c.getCharSetTableList().Contains(input[i]))
                        validCharacter = true;
                }
                // This is for cases of random tokens that aren't accepted in any transitions.
                // We just skip them here and be done with it.
                if (!validCharacter)
                {
                    FailPrint.Add(string.Format(F_form, failedTokenCount, "At", lineCount + ",", atCol + ":", input[i]));
                    AcceptPrint.Add(string.Format(A_form, tokenCount, "Error",
                        "Unrecognized character: " + input[i], "at " + lineCount + "," + atCol));
                    tokenCount++;
                    failedTokenCount++;
                    TStream.AddToken(new Token(input[i].ToString(), -1, false, atCol));
                    continue;
                }

                while(transition && i< input.Length)
                {
                    foreach (DFAEdge e in state.getEdgeList())
                    {
                        // This line is a bit messy, what it does is it checks the character set table
                        // and looks for the CharSetTableMember that corresponds to the which edge we're on.
                        // The char set members are unique corresponding to a specific state transition.
                        // It then pulls that list of characters stored in the CharSetTableMember
                        // And checks if the current character in our input is in there somewhere.
                        // In which case we add it onto our burgeoning token, change state and look at the next character.
                        if (GPtables.getCharSetTable()[e.getDFAedgeIndex()].getCharSetTableList().Contains(input[i]))
                        {
                            newstate = e.getDFAedgeTarget();
                            state = GPtables.getDFATable()[newstate];
                            token += input[i];
                            edgeCount = 0;
                            i++;
                            // We don't care about any other edges as a transition is based on character input
                            // and not final state, that is it will branch naturally once it hits characters
                            // that distinguish tokens such as . distinguishing a int from a float,exp,etc
                            break;
                        }
                        else
                        {
                            edgeCount++;
                        }
                    }
                    if (edgeCount == state.getEdgeCount())
                    {
                        transition = false;
                    }
                }
                // We don't check for accept states while building the token, because eventually it'll hit
                // a wall with state transitions at which point the tokenizing will fail.
                // So once we've built our token as big as we can we'll check for an accept state.

                // If it's not an error state, we check to make sure it's not whitespace (symbol==2)
                // And then move to add it to our token stream and printout
                if (state.getAcceptSymbolIndex() !=-1)
                {
                    symbol = state.getAcceptSymbolIndex();
                    if (symbol != 2)
                    {
                        string token_name = GPtables.getSymbolTable()[symbol].getSymbolTableName();
                        AcceptPrint.Add(string.Format(A_form, tokenCount, token_name, token, "at " + lineCount + "," + atCol));
                        TStream.AddToken(new Token(token, symbol, true, atCol));
                        tokenCount++;
                    }
                }

                else
                {
                    FailPrint.Add(string.Format(F_form, failedTokenCount, "At", lineCount + ",", atCol + ":", token));
                    AcceptPrint.Add(string.Format(A_form, tokenCount, "Error",
                        "Unrecognized character: " + token, "at " + lineCount + "," + atCol));
                    tokenCount++;
                    failedTokenCount++;
                    TStream.AddToken(new Token(token, -1, false, atCol));
                }
                newstate = initialstate;
                state = GPtables.getDFATable()[initialstate];
                token = "";

                // The reason we subtract is because we add in the above loop on the assumption
                // that the next piece of the input will be added to our token
                // When we get down here that last bit isn't added so we want to make sure we're checking that character
                // This is especially relevant for the last character in the input string as it would skip it otherwise continuing the loop.
                i--;
            }
            TStream.AddToken(new Token("EOF", 0, true, input.Length+1));
            return TStream;
		}
    }

}
