using System;
using System.Collections.Generic;

namespace CardGame {
    public class MonsterCard : Card {
        public int? Attack;
        public int? Health;

        public MonsterCard(int pId, string pName, int? pAttack, int? pHealth, string pDescription, int pRegCost, int pTurboCost, string pActions) 
            : base(pId, pName, pDescription, pRegCost, pTurboCost, pActions) {
            Attack = pAttack;
            Health = pHealth;
        }
    }
}


