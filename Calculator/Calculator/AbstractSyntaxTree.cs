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

            //foreach(Node n in Chld)
            //{
            //    if (this.Children == null)
            //        this.Children.AddFirst(n);
            //    else
            //        this.Children.AddLast(n);
            //}
            
            return Head;
        }

        public void TraverseTree()
        {
            Stack<Node> Next = new Stack<Node>();
            LinkedList<Node> Visited = new LinkedList<Node>();

            Next.Push(Head);

            while (Next.Count > 0)
            {
               /* foreach (Node k in Next)
                {
                    Console.Out.Write("Name: {0}", k.getToken().getTokenName());//, k.getType());
                }*/
                Console.Out.Write("{0} \n",Next.Peek().getToken().getTokenName());
                Visited.AddFirst(Head);
                foreach (Node n in Next.Pop().getChildren())
                {
                    Next.Push(n);
                    Console.Out.Write("{0} ", n.getToken().getTokenName());
                }
                Console.Out.Write("\n\n");
                //Next.Pop();

            }
        }

        public void TraverseTreeBF()
        {
            Queue<Node> Next = new Queue<Node>();
            LinkedList<Node> Visited = new LinkedList<Node>();

            Next.Enqueue(Head);

            while (Next.Count > 0)
            {
                /* foreach (Node k in Next)
                 {
                     Console.Out.Write("Name: {0}", k.getToken().getTokenName());//, k.getType());
                 }*/
                //Console.Out.Write("{0} \n", Next.Peek().getToken().getTokenName());
                Visited.AddFirst(Head);
                Node Parent = Next.Dequeue();
                foreach (Node n in Parent.getChildren())
                {
                    Next.Enqueue(n);
                    Console.Out.Write("{0} ", n.getToken().getTokenName());
                }
                Console.Out.Write("\n\n");
                //Next.Pop();

            }
        }

        public void Print()
        {
            Head.PrintTree(" ", true);
        }
        /*
        public Node make_2L_tree(Node op, Node left, Node right)
        {
            this.Head = op;
            op.setLeftChild(left);
            op.setRightChild(right);

            return this.Head;
        }

        public Node make_1L_tree(Node op, Node left)
        {
            this.Head = op;
            op.setLeftChild(left);

            return this.Head;
        }*/

    }
}
