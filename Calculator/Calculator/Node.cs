using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.XML;

namespace Calculator
{
    class Node
    {
        private Token Content;
        private LinkedList<Node> Children;

        public Node(Token C, LinkedList<Node> CH)
        {
            this.Content = C;
            foreach (Node n in CH)
            {
                this.Children = CH;
            }
        }

        public Node(Token C)
        {
            this.Content = C;
            this.Children = new LinkedList<Node>();
        }

        public void setChildren(LinkedList<Node> CH)
        {
            this.Children = CH;
        }

        public void addChild_E(Node CH)
        {
            this.Children.AddLast(CH);
        }

        public void addChild_B(Node LC)
        {
            this.Children.AddFirst(LC);
        }

        public bool hasChildren()
        {
            if (this.Children.Count > 0)
                return true;
            else
                return false;
        }

        public Token getToken()
        {
            return this.Content;
        }

        public string getType()
        {
            XMLParser xm = new XMLParser();
            xm.parseAll();
            GoldParserTables GPTables = xm.getGPBTables();
            List<SymbolTableMember> symTable = GPTables.getSymbolTable();
            return  symTable[this.Content.getTokenSymbol()].getSymbolTableName();
        }

        public LinkedList<Node> getChildren()
        {
            return this.Children;
        }
        //public void setRightChild(Node RC)
        //{
        //    this.RightChild = RC;
        //}

        public bool isTerminal()
        {
            if(this.Content.getTokenSymbol() == ( 8 | 9 | 10 | 11 | 17 ))
                return true;
            else
                return false;
        }

        public bool isOperator()
        {
            int[] OPcheck = { 3, 5, 6, 7, 13, 14, 15, 18 };
            foreach (int s in OPcheck)
            {
                if (this.Content.getTokenSymbol() == s)
                    return true;
            }
                
            return false;
        }

    }
}
