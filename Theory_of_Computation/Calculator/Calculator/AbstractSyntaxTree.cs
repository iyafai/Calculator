using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class AbstractSyntaxTree
    {
        private Node Head;

        class Node
        {
            private Token Content;
            private Node LeftChild;
            private Node RightChild;

            public Node(Token C, Node LC, Node RC)
            {
                this.Content = C;
                this.LeftChild = LC;
                this.RightChild = RC;
            }

            public Node(Token C)
            {
                this.Content = C;
                this.LeftChild = null;
                this.RightChild = null;
            }

            public void setLeftChild(Node LC)
            {
                this.LeftChild = LC;
            }

            public void setRightChild(Node RC)
            {
                this.RightChild = RC;
            }
        }

        public void make_tree(Node op, Node left, Node right)
        {
            this.Head = op;
            op.setLeftChild(left);
            op.setRightChild(right);
        }

    }
}
