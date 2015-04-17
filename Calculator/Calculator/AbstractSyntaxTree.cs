using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class AbstractSyntaxTree
    {
        private Node Head;

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

        public void TraverseTree()
        {
            Stack<Node> Next = new Stack<Node>();
            LinkedList<Node> Visited = new LinkedList<Node>();

            Next.Push(Head);

            while (Next.Count > 0)
            {
                Console.Out.Write("{0} \n",Next.Peek().getToken().getTokenName());
                Visited.AddFirst(Head);
                foreach (Node n in Next.Pop().getChildren())
                {
                    Next.Push(n);
                    Console.Out.Write("{0} ", n.getToken().getTokenName());
                }
                Console.Out.Write("\n\n");
            }
        }

        public void TraverseTreeBF()
        {
            Queue<Node> Next = new Queue<Node>();
            LinkedList<Node> Visited = new LinkedList<Node>();

            Next.Enqueue(Head);

            while (Next.Count > 0)
            {
                Visited.AddFirst(Head);
                Node Parent = Next.Dequeue();
                foreach (Node n in Parent.getChildren())
                {
                    Next.Enqueue(n);
                    Console.Out.Write("{0} ", n.getToken().getTokenName());
                }
                Console.Out.Write("\n\n");
            }
        }

        public void Print()
        {
            Head.PrintTree(" ", true);
        }

        public void Calculate(Dictionary<string, string> varList)
        {
            Stack<Node> CStack = new Stack<Node>();
            Stack<Node> VStack = new Stack<Node>();
            Stack<Node> Calc = new Stack<Node>();
            List<Node> visited = new List<Node>();
            //double var1 = 0, var2 = 0;
            double[] varValue = new double[2];
            string[] varInput = new string[2];
            double result = 0;
            Node temp = this.Head;
            //Dictionary<string, string> v1 = new Dictionary<string, string>();
            Token v1 = new Token();

            VStack.Push(temp);
            //CStack.Push(temp);
            while (VStack.Count > 0)
            {
                temp = VStack.Pop();
                if (!visited.Contains(temp))
                {
                    visited.Add(temp);
                    CStack.Push(temp);
                    foreach (Node n in temp.getChildren())
                    {
                        VStack.Push(n);
                    }
                }
            }

            while (CStack.Count > 0)
            {
                while (CStack.Peek().isNum())
                {
                    Calc.Push(CStack.Pop());
                }
                if (Calc.Count > 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        varInput[i] = Calc.Peek().getToken().getTokenName();

                        if (Calc.Peek().getToken().getTokenSymbol() == 10)
                        {
                            string st = "";
                            varList.TryGetValue(varInput[i], out st);
                            try
                            {
                                varValue[i] = Double.Parse(st);
                            }
                            catch (ArgumentNullException)
                            {
                                varValue[i] = 0;
                            }
                            Calc.Pop();
                        }
                        else if (Calc.Peek().getToken().getTokenSymbol() == 11)
                        {
                            varValue[i] = Double.Parse(Calc.Pop().getToken().getTokenName(), System.Globalization.NumberStyles.Integer);
                        }
                        else if (Calc.Peek().getToken().getTokenSymbol() != 8)
                        {
                            varValue[i] = Double.Parse(Calc.Pop().getToken().getTokenName());
                            //var2 = Double.Parse(Calc.Pop().getToken().getTokenName());
                        }
                        else
                        {
                            varValue[i] = Double.Parse(Calc.Pop().getToken().getTokenName(), System.Globalization.NumberStyles.Float);
                        }
                        //Calc.Pop();
                    }
                }
                if (CStack.Peek().getToken().getTokenSymbol() == 3)             //Addition
                {
                    result = (varValue[1] + varValue[0]);
                }
                else if (CStack.Peek().getToken().getTokenSymbol() == 5)        //divisor
                {
                    result = (int)(varValue[1] / varValue[0]);
                }
                else if (CStack.Peek().getToken().getTokenSymbol() == 6)        //divide
                {
                    result = (varValue[1] / varValue[0]);
                }
                else if (CStack.Peek().getToken().getTokenSymbol() == 7)        //Equals
                {
                    string val = System.Convert.ToString(varValue[0]);
                    varList.Add(varInput[1], val);
                }
                else if (CStack.Peek().getToken().getTokenSymbol() == 13)        //Modulo
                {
                    result = (varValue[1] % varValue[0]);
                }
                else if (CStack.Peek().getToken().getTokenSymbol() == 14)        //Multiplication
                {
                    result = (varValue[1] * varValue[0]);
                }
                else if (CStack.Peek().getToken().getTokenSymbol() == 15)        //Power
                {
                    result = Math.Pow(varValue[1],varValue[0]);
                }
                else if (CStack.Peek().getToken().getTokenSymbol() == 18)        //Subtraction
                {
                    result = (varValue[1] - varValue[0]);
                }
                //string res = "" + result;
                Token tempT = new Token(System.Convert.ToString(result), 9, true);
                Node tempN = new Node(tempT);
                CStack.Pop();
                Calc.Push(tempN);

            }
            Console.Out.Write("{0} = {1}\n", varInput[1], varInput[0]);
            //return v1;
        }

    }
}
