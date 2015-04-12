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
            Token Head = new Token("HEAD", 100, true);
            Node HeadN = new Node(Head);
            AbstractSyntaxTree AST = new AbstractSyntaxTree(HeadN);
            List<LALRState> lalrTable = GPTables.getLALRTable();
            List<RuleTableMember> prodTable = GPTables.getRuleTable();
            List<SymbolTableMember> symTable = GPTables.getSymbolTable();
            List<CharSetTableMember> charTable = GPTables.getCharSetTable();
            LALRState Lstate = lalrTable[0];
            Stack<int> State_Stack = new Stack<int>();
            Stack<Token> Input_Stack = new Stack<Token>();
            Stack<Node> Semantic_Stack = new Stack<Node>();

            int current_state = 0;
            State_Stack.Push(current_state);
            Token Current = TStream.getToken(current_state);
            Token Next = TStream.getNextToken(current_state);
            LALRAction Lact = Lstate.getActMatchesIndex(Current.getTokenSymbol());

            int i = 0;
            while(State_Stack.Count >0 || Input_Stack.Count > 0)
            {
                //Pulls state obj for current state (vert on table)
                Lstate = lalrTable[State_Stack.Peek()];             
                //Action corresponds to incoming input (horizontal on table
                Lact = Lstate.getActMatchesIndex(Current.getTokenSymbol());

                if (Lact.getAction() == 1)                      //Shift
                {
                    Node TempN = new Node(Current);
                    current_state = Lact.getValue();            //New state becomes the value of the action
                    State_Stack.Push(Lact.getValue());          //push onto stack
                    Input_Stack.Push(Current);                  //Input Token gets pushed on, wraps to beginning for next Token
                    Semantic_Stack.Push(TempN);
                    Current = TStream.getNextToken(i);
                    i++;
                }

                else if (Lact.getAction() == 2)                 //Reduce
                {
                    //grabs production with index given from LALR Action
                    int ProdInd = Lact.getValue();
                    //saves the Non-Terminal Index for creating a new token for pushing on the stack
                    int NTind = prodTable[ProdInd].getProd_NT_index();
                    //create New Token from the Non-Terminal Symbol
                    Token tempT = new Token(symTable[NTind].getSymbolTableName(), NTind, true);
                    LinkedList<Node> tempN = new LinkedList<Node>();
                    Node OPN = null;
                    bool first = false;

                    foreach (int sym in prodTable[ProdInd].getProd_symIndices())
                    {
                        if (Semantic_Stack.Peek().isOperator() && !first && !Semantic_Stack.Peek().hasChildren())
                        {
                            OPN = Semantic_Stack.Pop();
                            first = true;
                        }
                        else
                        {
                            tempN.AddFirst(Semantic_Stack.Pop());
                        }
                        Input_Stack.Pop();
                        State_Stack.Pop();
                    }
                    Input_Stack.Push(tempT);
                    if (prodTable[ProdInd].getProd_SymCount() == 3)
                    {
                        Semantic_Stack.Push(AST.makeTree(OPN, tempN));
                    }
                    else if(prodTable[ProdInd].getProd_SymCount() == 2)
                    {
                        Semantic_Stack.Push(AST.makeTree(OPN, tempN));
                    }
                    else //if(prodTable[ProdInd].getProd_SymCount() == 1)
                    {
                        Node temp2 = new Node(tempT);
                        Semantic_Stack.Push(AST.makeTree(temp2, tempN));
                        //Semantic_Stack.Push(tempN.ElementAt(0));
                    }

                    //The new state is what's left on the State Stack
                    Lstate = lalrTable[State_Stack.Peek()];
                    //After the Reduce we have a goto from the Non-Terminal Token
                    //Our current state becomes the Action Value from the LALR Action that matches the index
                    current_state = Lstate.getActMatchesIndex(NTind).getValue();
                    State_Stack.Push(current_state);
                }

                else if (Lact.getAction() == 3)     //Goto
                {
                    current_state = Lact.getValue();
                    State_Stack.Push(current_state);
                }

                else if (Lact.getAction() == 4)     //Accept
                {
                    AST.TraverseTree();
                    return AST;
                }

                else                                //Failure
                {
                    Console.Out.WriteLine("Parse Failed");
                    //Better error handling to come later
                }
            }

            return AST;
        }
    }
}
