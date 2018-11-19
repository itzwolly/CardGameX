namespace CardGame {
    using System;
    using System.Collections.Generic;

    public class Card {
        public readonly int Id;
        public readonly string Name;
        public readonly string Description;
        public readonly int RegCost;
        public readonly int TurboCost;
        public readonly string ActionsString;
        public readonly List<CardAction> Actions;

        public enum CardType {
            None,
            Spell,
            Monster
        }

        public Card(int pId, string pName, string pDescription, string pActions) {
            Id = pId;
            Name = pName;
            Description = pDescription;
            Actions = new List<CardAction>();

            ConvertActions(pActions);
        }

        public Card(int pId, string pName, string pDescription, int pRegCost, int pTurboCost, string pActions) {
            Id = pId;
            Name = pName;
            Description = pDescription;
            RegCost = pRegCost;
            TurboCost = pTurboCost;
            Actions = new List<CardAction>();

            ConvertActions(pActions);
        }

        public static CardType ValidateType(string pCardType) {
            CardType type = (CardType) Enum.Parse(typeof(CardType), pCardType);
            if (Enum.IsDefined(typeof(CardType), pCardType)) {
                return type;
            }
            return CardType.None;
        }

        private void ConvertActions(string pActions) {
            if (pActions == "" || pActions == null) {
                return;
            }

            if (pActions.Contains(",")) {
                string[] actions = pActions.Split(',');

                for (int i = 0; i < actions.Length; i++) {
                    string actionString = actions[i].Trim();
                    CardAction action = CardAction.CreateInstance(actionString);
                    Actions.Add(action);
                }
            } else {
                string actionString = pActions.Trim();
                CardAction action = CardAction.CreateInstance(actionString);
                Actions.Add(action);
            }
        }

        public List<EventResponse> Execute(PlayerState pPlayerState) {
            List<EventResponse> responses = new List<EventResponse>();
            foreach (CardAction action in Actions) {
                EventResponse response = action.Execute(pPlayerState);
                responses.Add(response);
            }
            return responses;
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