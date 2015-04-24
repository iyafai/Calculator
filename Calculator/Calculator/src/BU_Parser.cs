using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.XML;
using System.IO;
using Calculator.Tables;

namespace Calculator
{
    class BU_Parser
    {
        // Stuff for printing to the file
        private static List<string> parser_results = new List<string>();
        private static string userPathLoc = Environment.CurrentDirectory;
        private static string debugOutputPath = userPathLoc + @"\Output\debug\";

        public void printOutput(string path)
        {
            string result = debugOutputPath + path + "_Parsing.out";
            Console.Out.Write("Parse Results Printed to: {0}\n", result);
            File.WriteAllLines(result, parser_results);
            parser_results.Clear();
        }

        public void addStrToPrint(string add_str)
        {
            parser_results.Add(add_str);
        }

        public void addStrToPrint(List<string> add_strL)
        {
            parser_results.AddRange(add_strL);
        }

        public AbstractSyntaxTree ParseStream(TokenStream TStream)
        {
            Node HeadN = new Node();
            AbstractSyntaxTree AST = new AbstractSyntaxTree(HeadN);
            
            GoldParserTables GPTables = new GoldParserTables();
            List<LALRState> lalrTable = GPTables.getLALRTable();
            List<RuleTableMember> prodTable = GPTables.getRuleTable();
            List<SymbolTableMember> symTable = GPTables.getSymbolTable();
            List<CharSetTableMember> charTable = GPTables.getCharSetTable();
            
            string ast = new String('*', 50);
            string tokenErrorMsg = "^\n**Parsing Failed on Token of type [{0}]: {1}, at Col: {2}";

            LALRState Lstate = lalrTable[0];
            Stack<int> State_Stack = new Stack<int>();
            Stack<Token> Input_Stack = new Stack<Token>();
            Stack<Node> Semantic_Stack = new Stack<Node>();

            int current_state = 0;
            State_Stack.Push(current_state);
            Token Current = TStream.getToken(current_state);
            Token Next = TStream.getNextToken(current_state);
            LALRAction Lact = Lstate.getActMatchesIndex(Current.getTokenSymbol());

            int input_counter = 0;
            while (State_Stack.Count > 0 || Input_Stack.Count > 0)
            {
                //Pulls state obj for current state (vert on table)
                Lstate = lalrTable[State_Stack.Peek()];
                //Action corresponds to incoming input (horizontal on table
                Lact = Lstate.getActMatchesIndex(Current.getTokenSymbol());

                if(Lact == null)                                //Failure
                {
                    // Parsing hits a dead end, we throw an error here for the user
                    //Console.Out.WriteLine("Parse Failed");
                    string spaces = new String(' ', Current.getTokenLoc() - 1);
                    string E_message = "";
                    parser_results.Add(string.Format("Failure at State:{0} Pushing {1}", current_state, Current.getTokenName()));
                    parser_results.Add(ast);
                    if (Current.getTokenSymbol() == -1)
                    {
                        throw new ParseErrorException(
                            string.Format(spaces + tokenErrorMsg,"invalid", Current.getTokenName(), Current.getTokenLoc()));
                    }
                    else
                    {
                        string symbolname = Current.getTokenType();// symTable[Current.getTokenSymbol()].getSymbolTableName();
                        StringBuilder expected = new StringBuilder();
                        foreach (LALRAction la in Lstate.getLALR_ActionList())
                        {
                            if (la.getIndex() < 19)
                            {
                                expected.Append("[" + symTable[la.getIndex()].getSymbolTableName() + "] ");
                            }
                        }
                        throw new ParseErrorException(
                            string.Format(spaces + tokenErrorMsg + "\nExpected: {3}",
                            symbolname, Current.getTokenName(), Current.getTokenLoc(), expected.ToString()));
                    }
                    throw new ParseErrorException(E_message);
                    //Better error handling to come later
                }
                else if (Lact.getAction() == 1)                      //Shift
                {
                    parser_results.Add(string.Format("Shift at State:{0} Pushing {1}", current_state,Current.getTokenName()));
                    Node TempN = new Node(Current);
                    // Value from LALR table saved as next state.
                    current_state = Lact.getValue();            
                    State_Stack.Push(Lact.getValue());
                    Input_Stack.Push(Current);
                    // Everything gets pushed onto the semantic stack
                    // but useless characters are filtered later
                    Semantic_Stack.Push(TempN);
                    // Pull next Token from Input
                    Current = TStream.getNextToken(input_counter);
                    input_counter++;
                }

                else if (Lact.getAction() == 2)                 //Reduce
                {
                    // grabs production rule with index given by LALR Action
                    int ProdInd = Lact.getValue();

                    // Creates a new token from the Non-Terminal Symbol
                    int NTind = prodTable[ProdInd].getProd_NT_index();
                    Token nonTerminalToken = new Token(symTable[NTind].getSymbolTableName(), NTind, true);
                    Node nonTerminalNode = new Node(nonTerminalToken);
                    // Stores Children (if any) for new tree
                    LinkedList<Node> productionTerminals = new LinkedList<Node>();
                    // Stores Operator for use as root of new tree
                    Node operatorNode = new Node();

                    parser_results.Add(string.Format("Reduce at State: {0}, [{1}] replaces: {2} terminals:", current_state, 
                        nonTerminalToken.getTokenType(), prodTable[ProdInd].getProd_symIndices().Count));

                    // Here is where we do the reduction itself
                    foreach (int sym in prodTable[ProdInd].getProd_symIndices())
                    {
                        parser_results.Add(string.Format("\t\t{0} ", symTable[sym].getSymbolTableName()));
                        // For our current tables, if our terminals only number 1
                        // then it's unneccessary to add to the semantic stack as it's just
                        // replacing a terminal with a non-terminal which we don't want in the tree
                        if (prodTable[ProdInd].getProd_SymCount() > 1)
                        {
                            // Our stack can have multiple operators waiting (from other subtrees)
                            // We don't want these subtrees to become the new root hence this check
                            if (Semantic_Stack.Peek().isOperator() && !Semantic_Stack.Peek().hasChildren())
                            {
                                operatorNode = Semantic_Stack.Pop();
                            }
                            else
                            {
                                productionTerminals.AddLast(Semantic_Stack.Pop());
                            }
                        }
                        Input_Stack.Pop();
                        State_Stack.Pop();
                    }

                    // We need the nonTerminal for the Parsing to work
                    Input_Stack.Push(nonTerminalToken);

                    // The following If-Else creates a new tree based on the reduction.
                    // Checking first for the special case of negative numbers.
                    // When a negative is encountered it places it on the tree as -1 * number.
                    if (prodTable[ProdInd].getProd_SymCount() == 2 && prodTable[ProdInd].getProd_symIndices().Contains(18))
                    {
                        operatorNode = new Node(new Token("*", 14, true));
                        productionTerminals.AddLast(new Node(new Token("-1", 9, true)));
                        Semantic_Stack.Push(AST.makeTree(operatorNode, productionTerminals));
                    }
                    
                    // We ignore SymCount==1 as it's just a NonTerminal subbing for a Terminal.
                    else if (prodTable[ProdInd].getProd_SymCount() > 1)
                    {
                        // First We trim the unnecessary characters: ( ) , ;
                        // As they have no bearing on the actual calculation once parsing is done.
                        LinkedList<Node>newChldrn = new LinkedList<Node>();
                        foreach (Node n in productionTerminals)
                        {
                            if (!n.isUnNeeded())
                            {
                                newChldrn.AddFirst(n);
                            }
                        }

                        // This is used to distinguish an expression that is represented as (expression).
                        if (newChldrn.Count > 1)
                        {
                            // Might be unneeded, check later
                            if (operatorNode == null)
                            {
                                Semantic_Stack.Push(AST.makeTree(nonTerminalNode, newChldrn));
                            }
                            else
                            {
                                Semantic_Stack.Push(AST.makeTree(operatorNode, newChldrn));
                            }
                        }
                        // In the case of ( expression ) we just return our expression to the stack w/o parens.
                        else
                        {
                            Semantic_Stack.Push(newChldrn.ElementAt(0));
                        }
                    }

                    // Reductions are followed by a go-to statement
                    Lstate = lalrTable[State_Stack.Peek()];
                    // After the Reduce we have a goto from the Non-Terminal Token
                    // Our new state becomes the Action Value from the LALR Action that matches the index
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
                    // Parsing is successful, returns the Abstract Syntax Tree
                    parser_results.Add(string.Format("Accept State at State={0}",current_state));
                    parser_results.Add(ast);
 
                    //AST.Print();
                    return AST;
                }
            }

            return AST;
        }
    }
}
