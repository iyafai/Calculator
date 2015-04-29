using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Calculator;
using Calculator.Tables;

namespace Calculator.XML
{
    class GoldParserTables
    {
        // Only need the one instance
        private static List<SymbolTableMember> SymbolTable = new List<SymbolTableMember>();
        private static List<RuleTableMember> RuleTable = new List<RuleTableMember>();
        private static List<CharSetTableMember> CharSetTable = new List<CharSetTableMember>();
        private static List<DFAState> DFATable = new List<DFAState>();
        private int initialDFAState = 0;
        private static List<LALRState> LALRTable = new List<LALRState>();
        
        public GoldParserTables()
        {/*
            SymbolTable = new List<SymbolTableMember>();
            RuleTable = new List<RuleTableMember>();
            CharSetTable = new List<CharSetTableMember>();
            DFATable = new List<DFAState>();
            initialDFAState = 0;
            LALRTable = new List<LALRState>();*/
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
