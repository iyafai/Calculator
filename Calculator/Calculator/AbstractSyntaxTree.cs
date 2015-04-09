using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class AbstractSyntaxTree
    {
        private Node Head;
        private LinkedList<Node> Children;

        public AbstractSyntaxTree(Node H)
        {
            this.Head = H;
            this.Children = new LinkedList<Node>();
        }

        public Node makeTree(Node OP, List<Node> Chld)
        {
            this.Head = OP;
            foreach(Node n in Chld)
            {
                if (this.Children == null)
                    this.Children.AddFirst(n);
                else
                    this.Children.AddLast(n);
            }
            
            return Head;
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
