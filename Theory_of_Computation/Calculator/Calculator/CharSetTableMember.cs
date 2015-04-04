using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    class CharSetTableMember
    {
        private int index;
        private int count;
        private List<char> charList;

        public CharSetTableMember(int index, int count, List<char> charList)
        {
            // TODO: Complete member initialization
            this.index = index;
            this.count = count;
            this.charList = charList;
        }

        public int getCharSetTableIndex()    {  return index; }
        public int getCharSetTableCount()    { return count; }
        public List<char> getCharSetTableList()    { return charList; }
    }
}
