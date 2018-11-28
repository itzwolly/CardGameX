using System;
using System.Collections.Generic;

namespace CardGame {
    public class SpellCard : Card {
        public SpellCard(int pId, string pName, string pDescription, int pRegCost, int pTurboCost, byte[] pActions, bool pIsTurbo) 
            : base(pId, pName, pDescription, pRegCost, pTurboCost, pActions, pIsTurbo) { }
    }
}