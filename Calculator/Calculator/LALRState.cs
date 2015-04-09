using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class LALRState
    {
        private int index;
        private int actionCount;
        private List<LALRAction> LALRActionList;

        public LALRState(int index, int actionCount, List<LALRAction> LALRActionList)
        {
            // TODO: Complete member initialization
            this.index = index;
            this.actionCount = actionCount;
            this.LALRActionList = LALRActionList;
        }

        public int getLALR_index() { return index; }
        public int getLALR_actionCount() {   return actionCount; }
        public List<LALRAction> getLALR_ActionList()   {   return LALRActionList; }
        public LALRAction getActMatchesIndex(int sym)
        {
            int count = 0;
            foreach (LALRAction L in this.getLALR_ActionList())
            {
                if (L.getIndex() == sym)
                    break;
                else
                    count++;
            }
            try
            {
                return this.getLALR_ActionList()[count];
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Out.Write("Error Index Out of Bounds on Checking LALR Table LALR_index:{0}, Action_Count: {1} ", this.getLALR_index(),
                    this.getLALR_actionCount());
                return null;
            }
            //return this.getLALR_ActionList()[count];
        }
    }
}
