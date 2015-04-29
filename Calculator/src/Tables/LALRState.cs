using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Tables
{
    class LALRState
    {
        private int index;
        private int actionCount;
        private List<LALRAction> LALRActionList;

        public LALRState(int index, int actionCount, List<LALRAction> LALRActionList)
        {
            this.index = index;
            this.actionCount = actionCount;
            this.LALRActionList = LALRActionList;
        }

        public int getLALR_index() { return index; }
        public int getLALR_actionCount() {   return actionCount; }
        public List<LALRAction> getLALR_ActionList()   {   return LALRActionList; }

        // Each item in LALRActionList has their own index, which corresponds to a token.
        // This pulls the LALRAction that has that value as its index.
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
            catch (ArgumentOutOfRangeException)
            {
                // This gets handled in the parser where it throws a ParseException
                return null;
            }
        }
    }
}
