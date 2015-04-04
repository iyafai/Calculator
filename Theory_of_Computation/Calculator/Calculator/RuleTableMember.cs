using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class RuleTableMember
    {
        private int index;
        private int nonterminalIndex;
        private int symbolCount;
        private List<int> symbolIndices;

        public RuleTableMember(int index, int nonterminalIndex, int symbolCount, List<int> symbolIndices)
        {
            // TODO: Complete member initialization
            this.index = index;
            this.nonterminalIndex = nonterminalIndex;
            this.symbolCount = symbolCount;
            this.symbolIndices = symbolIndices;
        }
    }
}
