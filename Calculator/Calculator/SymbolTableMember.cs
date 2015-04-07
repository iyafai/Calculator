using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class SymbolTableMember
    {
        private int index;
        private string name;
        private int kind;

        public SymbolTableMember(int index, string name, int kind)
        {
            // TODO: Complete member initialization
            this.index = index;
            this.name = name;
            this.kind = kind;
        }

        public int getSymbolTableIndex() { return index; }
        public string getSymbolTableName() { return name; }
        public int getSymbolTablekind() { return kind; }
    }
}
