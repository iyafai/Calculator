using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Calculator;

namespace Calculator.XML
{
    class GoldParserTables
    {
        private List<SymbolTableMember> SymbolTable;
        private List<RuleTableMember> RuleTable;
        private List<CharSetTableMember> CharSetTable;
        private List<DFAState> DFATable;
        private int initialDFAState;
        private List<LALRState> LALRTable;

        public GoldParserTables()
        {
            SymbolTable = new List<SymbolTableMember>();
            RuleTable = new List<RuleTableMember>();
            CharSetTable = new List<CharSetTableMember>();
            DFATable = new List<DFAState>();
            initialDFAState = 0;
            LALRTable = new List<LALRState>();
        }

        //SymbolTable Accessors
        public void setSymbolTable(List<SymbolTableMember> newList)
        {
            SymbolTable = newList;
        }
        public List<SymbolTableMember> getSymbolTable()
        {
            return SymbolTable;
        }

        //RuleTable Accessors
        public void setRuleTable(List<RuleTableMember> newList)
        {
            RuleTable = newList;
        }
        public List<RuleTableMember> getRuleTable()
        {
            return RuleTable;
        }

        //CharSetTable Accessors
        public void setCharSetTable(List<CharSetTableMember> newList)
        {
            CharSetTable = newList;
        }
        public List<CharSetTableMember> getCharSetTable()
        {
            return CharSetTable;
        }

        //DFATable Accessors
        public void setDFATable(List<DFAState> newList)
        {
            DFATable = newList;
        }
        public List<DFAState> getDFATable()
        {
            return DFATable;
        }
        public void setInitialDFAState(int init)
        {
            initialDFAState = init;
        }
        public int getInitialDFAState()
        {
            return initialDFAState;
        }

        //LALRTable Accessors
        public void setLALRTable(List<LALRState> newList)
        {
            LALRTable = newList;
        }
        public List<LALRState> getLALRTable()
        {
            return LALRTable;
        }
    }
}
