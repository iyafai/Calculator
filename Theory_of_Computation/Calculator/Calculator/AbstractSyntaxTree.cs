using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class AbstractSyntaxTree
    {
        private Node Head;
        public void make_tree(Node op, Node left, Node right)
        {
            this.Head = op;
            op.setLeftChild(left);
            op.setRightChild(right);
        }

    }
}
