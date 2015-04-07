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
            Stack<int> State_Stack = new Stack<int>();
            Stack<Token> Input_Stack = new Stack<Token>();
            Stack<Node> Semantic_Stack = new Stack<Node>();
            int current_state = 0;//TStream.getToken(0).getTokenSymbol();
            State_Stack.Push(current_state);

            for(int i=0; i<TStream.Count(); i++)
            {
                Token Next = TStream.getNextToken(i);
                Lstate = lalrTable[current_state];
                LALRAction Lact = Lstate.getLALR_ActionList()[Next.getTokenSymbol()];

//                foreach (LALRAction Lact in Lstate.getLALR_ActionList())
//                {
                    
//                    if (Next.getTokenSymbol() == Lact.getIndex())
//                    {
                        if (Lact.getAction() == 1)          //Shift
                        {
                            current_state = Lact.getValue();
                            State_Stack.Push(Lact.getValue());
                            Input_Stack.Push(Next);
                            continue;
                        }

                        else if (Lact.getAction() == 2)     //Reduce
                        {
                            List<Node> temp = new List<Node>();
                            Node OPN = null;
                            foreach (int j in prodTable[current_state].getProd_symIndices())
                            {
                                if (Input_Stack.Peek().isOperator())
                                {
                                    OPN = new Node(Input_Stack.Pop());
                                }
                                else if(Input_Stack.Peek().isTerminal())
                                {
                                    Node tempN = new Node(Input_Stack.Pop());
                                    temp.Add(tempN);
                                }
                            }
                            int NTind = prodTable[current_state].getProd_NT_index();
                            Token tempT = new Token(symTable[NTind].getSymbolTableName(),NTind,true);
                            Input_Stack.Push(tempT);
                            AST.make_tree(OPN, temp[0], temp[1]);
                            current_state = State_Stack.Peek();
                            Lstate = lalrTable[current_state];
                            current_state = Lstate.getLALR_ActionList()[current_state].getAction();
                            State_Stack.Push(current_state);
                            continue;
                        }

                        else if (Lact.getAction() == 3)     //Goto
                        {
                            current_state = Lact.getValue();
                            State_Stack.Push(current_state);
                        }

                        else if (Lact.getAction() == 4)     //Accept
                        {
                        }

                        else                                //Failure
                        {
                        }
//                    }
//                }
            }

            return AST;
        }
    }
}
