using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class DFAEdge
    {
        private int charSetIndex;
        private int target;

        public DFAEdge()
        {
            charSetIndex = 0;
            target = 0;
        }

        public DFAEdge(int charSetIndex, int target)
        {
            this.charSetIndex = charSetIndex;
            this.target = target;
        }

        public int getDFAedgeIndex() { return charSetIndex; }
        public int getDFAedgeTarget() { return target; }
    }
}
