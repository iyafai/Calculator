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
            while (State_Stack.Count > 0 || Input_Stack.Count > 0)
            {
                //Pulls state obj for current state (vert on table)
                Lstate = lalrTable[State_Stack.Peek()];
                //Action corresponds to incoming input (horizontal on table
                Lact = Lstate.getActMatchesIndex(Current.getTokenSymbol());

                if (Lact.getAction() == 1)                      //Shift
                {
                    Console.Out.Write("Shift Operation Pushing {0} going to state {1}\n", Current.getTokenName(),Lact.getValue());
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
                    Node tempF = new Node(tempT);
                    LinkedList<Node> tempN = new LinkedList<Node>();
                    Node OPN = null;
                    bool first = false;
                    int[] skip = { 4, 12, 16, 17, 19, 20, 21, 22, 23, 24, 25 };
                    Console.Out.Write("Reduce Operation at State: {2} {0} elements, {1} NT replaces: ", 
                        prodTable[ProdInd].getProd_symIndices().Count, symTable[NTind].getSymbolTableName(), current_state);
                    foreach (int sym in prodTable[ProdInd].getProd_symIndices())
                    {
                        Console.Out.Write(" {0} ", symTable[sym].getSymbolTableName());
                        if (Semantic_Stack.Peek().isOperator() && !Semantic_Stack.Peek().hasChildren())
                        {
                            OPN = Semantic_Stack.Pop();
                        }
                        else
                        {
                            tempN.AddFirst(Semantic_Stack.Pop());
                        }
                        Input_Stack.Pop();
                        State_Stack.Pop();
                    }

                    Console.Out.Write("\n");
                    Input_Stack.Push(tempT);
                    if (prodTable[ProdInd].getProd_SymCount() > 1)
                    {
                        if (OPN == null)
                        {
                            Semantic_Stack.Push(AST.makeTree(tempF,tempN));
                        }
                        else
                        {
                            Semantic_Stack.Push(AST.makeTree(OPN,tempN));
                        }
                    }
                    else if(prodTable[ProdInd].getProd_SymCount() == 1)
                    {
                        Semantic_Stack.Push(AST.makeTree(tempF, tempN));
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
                    Console.Out.Write("Accept State with State={0}\n",current_state);
                    //AST.TraverseTreeBF();
                    AST.Print();
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
