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
            this.index = index;
            this.nonterminalIndex = nonterminalIndex;
            this.symbolCount = symbolCount;
            this.symbolIndices = symbolIndices;
        }

        public int getProd_index() { return index; }
        public int getProd_NT_index() { return nonterminalIndex; }
        public int getProd_SymCount() { return symbolCount; }
        public List<int> getProd_symIndices() { return symbolIndices; }
    }
}
