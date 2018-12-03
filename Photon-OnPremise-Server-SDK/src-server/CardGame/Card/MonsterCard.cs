using System;
using System.Collections.Generic;

namespace CardGame {
    public class MonsterCard : Card, IInteractable {
        public int? Attack;
        public int? Health;

        public int BoardIndex {
            get;
            set;
        }

        public MonsterCard(int pId, string pName, int? pAttack, int? pHealth, string pDescription, int pRegCost, int pTurboCost, bool pIsTurbo)
            : base(pId, pName, pDescription, pRegCost, pTurboCost, pIsTurbo) {
            Attack = pAttack;
            Health = pHealth;
        }

        public int GetId() {
            return Id;
        }

        public int? GetHealth() {
            return Health;
        }

        public int? GetAttack() {
            return Attack;
        }

        public void SetHealth(int pNewHealth) {
            if (pNewHealth < 0) {
                Health = 0;
                // TODO: Add to death dictionary..
                return;
            }
            Health = pNewHealth;
        }

        public void SetAttack(int pAmount) {
            if (pAmount < 0) {
                Attack = 0;
                return;
            }
            Attack = pAmount;
        }

        public int GetOwnerId() {
            return OwnerId;
        }

        public int GetBoardIndex() {
            return BoardIndex;
        }
    }
}


