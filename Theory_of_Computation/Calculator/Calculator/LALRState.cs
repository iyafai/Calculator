﻿using System;
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
    }
}