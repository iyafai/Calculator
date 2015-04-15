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
        /*
        public AbstractSyntaxTree trimTree()
        {

        }*/
        /*
        public AbstractSyntaxTree buildTree(Stack<Node> SemanticStack)
        {
            LinkedList<Node> temp = new LinkedList<Node>();
            AbstractSyntaxTree A = this;
            while (SemanticStack.Count > 1)
            {
                if (SemanticStack.Peek().isNum() || (SemanticStack.Peek().isOperator() && SemanticStack.Peek().hasChildren()))
                {
                    temp.AddFirst(SemanticStack.Pop());
                }
                else if (SemanticStack.Peek().isOperator() && temp.Count>1)
                {
                    SemanticStack.Push(A.makeTree(SemanticStack.Pop(), temp));
                }
                else if (SemanticStack.Peek().isOperator())
                {
                    Node OPN = SemanticStack.Pop();
                    temp.AddFirst(SemanticStack.Pop());
                    SemanticStack.Push(A.makeTree(OPN, temp));
                }

            }
            A.makeTree(A.Head, temp);
            return A;
        }*/
    }
}
