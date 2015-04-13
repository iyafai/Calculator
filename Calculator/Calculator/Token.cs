using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class Token
    {
        private string token_name;
        private int symbol;
        private bool valid;

        public Token()
        {
            token_name = "";
            symbol = 0;
            valid = false;
        }

        public Token(string name, int sym, bool val)
        {
            token_name = name;
            symbol = sym;
            valid = val;
        }

        public void setTokenName(string name)    { token_name = name; }
        public void setTokenSymbol(int sym)      { symbol = sym; }
        public void setTokenValid() { valid = true; }
        public void setTokenInValid() { valid = false; }
        public bool isTokenValid() { return valid; }
        public string getTokenName()    {   return token_name;  }
        public int getTokenSymbol()    { return symbol; }

        public bool isOperator()
        {
            int[] OPcheck = { 3, 5, 6, 7, 13, 14, 15, 18 };
            foreach (int s in OPcheck)
            {
                if (this.getTokenSymbol() == s)
                    return true;
            }

            return false;
        }

        public bool isTerminal()
        {
            if (this.getTokenSymbol() == (8 | 9 | 10 | 11 | 17))
                return true;
            else
                return false;
        }
    }

    class TokenStream
    {
        private List<Token> TStream;

        public TokenStream()
        {
            TStream = new List<Token>();
        }

        public void AddToken(Token temp)
        {
            TStream.Add(temp);
        }

        public int Count() { return TStream.Count; }

        public Token getToken(int index) { return TStream[index]; }

        public Token getNextToken(int index)
        {
            Token EOS = new Token("EOS", 0, true);
            if (index >= (TStream.Count - 1))
                return EOS;
            else
                return TStream[index + 1];
        }
    }
}
