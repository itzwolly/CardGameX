using System;
using System.Collections.Generic;

namespace CardGame {
    public class SpellCard : Card, IInteractable {
        public SpellCard(int pId, string pName, string pDescription, int pRegCost, int pTurboCost, bool pIsTurbo)
            : base(pId, pName, pDescription, pRegCost, pTurboCost, pIsTurbo) {
        }

        public int? GetAttack() {
            return -1;
        }

        public int GetBoardIndex() {
            return -1;
        }

        public int? GetHealth() {
            return -1;
        }

        public int GetId() {
            return Id;
        }

        public string GetName() {
            return Name;
        }

        public int GetOwnerId() {
            return OwnerId;
        }

        public void SetAttack(int pAmount) {
            return;
        }

        public void SetHealth(int pAmount) {
            return;
        }
    }
}