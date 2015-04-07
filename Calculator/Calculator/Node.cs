using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
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

        public bool isTerminal()
        {
            if(this.Content.getTokenSymbol() == ( 8 | 9 | 10 | 11 | 17 ))
                return true;
            else
                return false;
        }
    }
}
