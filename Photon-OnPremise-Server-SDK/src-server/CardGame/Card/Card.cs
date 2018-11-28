namespace CardGame {
    using System;
    using System.Collections.Generic;

    public class Card {
        public readonly int Id;
        public readonly string Name;
        public readonly string Description;
        public int RegCost;
        public readonly int TurboCost;
        public readonly string ActionsString;
        public readonly bool IsTurbo;
        public readonly byte[] Actions;
        
        public int OwnerId {
            get;
            set;
        }

        public enum CardType {
            None,
            Spell,
            Monster
        }

        public Card(int pId, string pName, string pDescription, int pRegCost, int pTurboCost, byte[] pActions, bool pIsTurbo) {
            Id = pId;
            Name = pName;
            Description = pDescription;
            RegCost = pRegCost;
            TurboCost = pTurboCost;
            IsTurbo = pIsTurbo;
            Actions = pActions;
        }

        public static CardType ValidateType(string pCardType) {
            CardType type = (CardType) Enum.Parse(typeof(CardType), pCardType);
            if (Enum.IsDefined(typeof(CardType), pCardType)) {
                return type;
            }
            return CardType.None;
        }

        public Type GetCardType() {
            if (this is MonsterCard) {
                return typeof(MonsterCard);
            } else {
                return typeof(SpellCard);
            }
        }

        public bool IsMonster() {
            return this is MonsterCard;
        }
    }
}