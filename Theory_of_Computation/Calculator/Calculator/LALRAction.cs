using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class LALRAction
    {
        private int symbolIndex;
        private int action;
        private int value;

        public LALRAction(int symbolIndex, int action, int value)
        {
            // TODO: Complete member initialization
            this.symbolIndex = symbolIndex;
            this.action = action;
            this.value = value;
        }

        public int getIndex() { return symbolIndex; }
        public int getAction()  { return action; }
        public int getValue()   {   return value; }
    }
}
