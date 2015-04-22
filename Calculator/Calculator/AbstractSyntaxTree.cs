﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class AbstractSyntaxTree
    {
        private Node Head;
        private string divZeroErrorMsg = "**Error: Division by Zero. ({0}/{1}) is undefined. Result set to 0.";
        private string intdivZeroErrorMsg = "**Error: Division by Zero. Div({0},{1}) is undefined. Result set to 0.";
        private string modZeroErrorMsg = "**Error: Modulo by Zero. Mod({0},{1}) is undefined. Result set to 0.";
        private string variableDefinedWarning = "**Warning: Variable already defined. Future calculations will use new value";
        //private string undefinedResultErrorMsg = "**Calculation Error. Value of {0} is undefined. ";

        public AbstractSyntaxTree(Node H)
        {
            this.Head = H;
        }

        public Node makeTree(Node OP, LinkedList<Node> Chld)
        {
            this.Head = OP;
            this.Head.setChildren(Chld);
            return Head;
        }
        /*
        public void Print()
        {
            Head.PrintTree(" ", true);
        }*/

        public void Calculate(Dictionary<string, string> varList)
        {
            // searchStack, visited and treeLoc are used for traversing the tree with a Depth-First Search.
            // Nodes are stored in rpnStack in Reverse Polish Notation.
            // Allowing the use of the postfix algorith to solve for a value.
            // CalcStack taking the place of the stack in that algorithm.
            Node treeLoc = this.Head;
            List<Node> visited = new List<Node>();
            Stack<Node> searchStack = new Stack<Node>();
            Stack<Node> rpnStack = new Stack<Node>();
            Stack<Node> calcStack = new Stack<Node>();

            // varInput is used for storing and pulling variable names.
            // varValue is for values used in the calculation.
            double[] varValue = new double[2];
            string[] varInput = new string[2];
            double result = 0;

            searchStack.Push(treeLoc);
            while (searchStack.Count > 0)
            {
                treeLoc = searchStack.Pop();
                if (!visited.Contains(treeLoc))
                {
                    visited.Add(treeLoc);
                    rpnStack.Push(treeLoc);
                    foreach (Node n in treeLoc.getChildren())
                    {
                        searchStack.Push(n);
                    }
                }
            }

            // Algorithm overview:
            // Pulls numbers until it hits an operator
            // pushes back onto stack the result of the operation on the top 2 numbers on the stack
            while (rpnStack.Count > 0)
            {
                // RPN calculation, pulls numbers until it hits an operator
                while (rpnStack.Peek().isNum())
                {
                    calcStack.Push(rpnStack.Pop());
                }

                if (calcStack.Count > 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        varInput[i] = calcStack.Peek().getToken().getTokenName();

                        // In the case of a variable as an operand
                        if (calcStack.Peek().getToken().getTokenSymbol() == 10)
                        {
                            string st = "";
                            // First tries pulling the variable from our dictionary
                            varList.TryGetValue(varInput[i], out st);
                            try
                            {
                                // We try parsing it to a double, if it's NaN or Infinity we set it as 0.
                                varValue[i] = Double.Parse(st);
                                if (Double.IsInfinity(varValue[i]) || Double.IsNaN(varValue[i]))
                                {
                                    varValue[i] = 0;
                                }
                            }
                            // In the case of a null (i.e. the variable isn't already set) it defaults to 0.
                            catch (ArgumentNullException)
                            {
                                varValue[i] = 0;
                            }
                            calcStack.Pop();
                        }
                        // Pulls Integer off Stack Converts to Double
                        else if (calcStack.Peek().getToken().getTokenSymbol() == 11)
                        {
                            varValue[i] = Double.Parse(calcStack.Pop().getToken().getTokenName(), System.Globalization.NumberStyles.Integer);
                        }
                        // Everything else is considered a float (exp, decimal, negative number)
                        else 
                        {
                            string input = calcStack.Pop().getToken().getTokenName();
                            varValue[i] = Double.Parse(input, System.Globalization.NumberStyles.Float);
                        }
                    }
                }

                // Checking operator and performing operation.
                switch (rpnStack.Peek().getToken().getTokenSymbol())
                {
                    case 3:
                        // Addition
                        result = (varValue[1] + varValue[0]);
                        break;
                    case 5:    
                        // Integer Division.
                        if (Math.Abs(Math.Floor(varValue[0])) == 0)
                        {
                            throw new CalculationErrorException(String.Format(intdivZeroErrorMsg, varInput[1], varInput[0]));
                        }
                        else
                        {
                            result = (int)(Math.Abs(varValue[1]) / Math.Floor(Math.Abs(varValue[0])));
                        }
                        break;
                    case 6:
                        // Division
                        if (varValue[0] == 0)
                        {
                            throw new CalculationErrorException(String.Format(divZeroErrorMsg,varValue[1],varValue[0]));
                        }
                        result = (varValue[1] / varValue[0]);
                        break;
                    case 7:
                        // Equals
                        if (Double.IsInfinity(varValue[0]) || Double.IsNaN(varValue[0]))
                        {
                            varValue[0] = 0;
                        }

                         string val = System.Convert.ToString(varValue[0]);
                         try
                         {
                             varList.Add(varInput[1], val);
                         }
                         catch (ArgumentException e)
                         {
                             varList.Remove(varInput[1]);
                             varList.Add(varInput[1], val);
                             throw new CalculationErrorException(variableDefinedWarning);
                         }
                        
                        break;
                    case 13:
                        // Modulo
                        if (Math.Floor(varValue[1]) == 0 || Double.IsNaN(Math.Floor(varValue[1])))
                        {
                            throw new CalculationErrorException(String.Format(modZeroErrorMsg, varValue[1], varValue[0]));
                        }
                        result = Math.Abs((varValue[1]) % Math.Floor(varValue[0]));
                        break;
                    case 14:
                        // Multiplication
                        result = (varValue[1] * varValue[0]);
                        break;
                    case 15:
                        // Power
                        result = Math.Pow(varValue[1],varValue[0]);
                        break;
                    case 18:
                        // Subtraction
                        result = (varValue[1] - varValue[0]);
                        break;
                }
                // Can't forget to pop the operator currently on the stack
                rpnStack.Pop();
                // Converts result and pushes back onto stack for next calculation
                Node resultNode = new Node(new Token(System.Convert.ToString(result), 9, true));
                calcStack.Push(resultNode);
            }
        }

    }
}
