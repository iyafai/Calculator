using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.XML;

namespace Calculator
{
    class BU_Parser
    {
        public AbstractSyntaxTree ParseStream(GoldParserTables GPTables, TokenStream TStream)
        {
            AbstractSyntaxTree AST = new AbstractSyntaxTree();
            List<LALRState> lalrTable = GPTables.getLALRTable();
            List<RuleTableMember> prodTable = GPTables.getRuleTable();
            List<SymbolTableMember> symTable = GPTables.getSymbolTable();
            List<CharSetTableMember> charTable = GPTables.getCharSetTable();
            LALRState Lstate = lalrTable[0];

            for(int i=0; i<TStream.Count(); i++)
            {
                Token Next = TStream.getNextToken(i);
                Lstate = lalrTable[TStream.getToken(i).getTokenSymbol()];

                foreach (LALRAction Lact in Lstate.getLALR_ActionList())
                {
                    if (Next.getTokenSymbol() == Lact.getIndex())
                    {

                    }
                }
            }

            return AST;
        }
    }
}
