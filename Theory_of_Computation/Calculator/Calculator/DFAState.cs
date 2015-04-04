using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class DFAState
    {
        int idx;
        int eCnt;
        int accSym;
        List<DFAEdge> eLst;

        public DFAState(int index, int edgeCount, int acceptSymbol, List<DFAEdge> edgeList)
        {
            idx = index;
            eCnt = edgeCount;
            accSym = acceptSymbol;
            eLst = edgeList;
        }

        public int getIndex()
        {
            return idx;
        }

        public int getEdgeCount()
        {
            return eCnt;
        }

        public int getAcceptSymbolIndex()
        {
            return accSym;
        }

        public List<DFAEdge> getEdgeList()
        {
            return new List<DFAEdge>(eLst);
        }


    }
}
