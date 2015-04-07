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
        public TokenStream getTokenStream(GoldParserTables GPtables, string input)
        {
            int init_state = GPtables.getInitialDFAState();         //convenience for going back to beginning of DFA
			int state_tran = init_state;                            //keeps track of where we're going (initial value doesn't matter)
			DFAState state = GPtables.getDFATable()[init_state];    //state to begin with

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
                    test.setTokenName(input[i].ToString());
                    test.setTokenSymbol(-1);
                    test.setTokenInValid();
                    TStream.AddToken(test);
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
					if (state.getAcceptSymbolIndex() == -1)     //No acceptance, token is added to error printout
					{
                        test.setTokenName(token);
                        test.setTokenSymbol(-1);
                        test.setTokenInValid();
                        TStream.AddToken(test);
						state = GPtables.getDFATable()[init_state];
					}

					else                                        //Accept state
					{
						symbol = state.getAcceptSymbolIndex();
						if (symbol != 2)                        //who cares about white space anyhow
						{
                            test.setTokenName(token);
                            test.setTokenSymbol(symbol);
                            test.setTokenInValid();
                            TStream.AddToken(test);
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
                        test.setTokenName(token);
                        test.setTokenSymbol(symbol);
                        test.setTokenInValid();
                        TStream.AddToken(test);
					}
				}
			}
            return TStream;
		}
    }

}
