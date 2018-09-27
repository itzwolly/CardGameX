namespace CardGame {
    using System.Collections;
    using System.Collections.Generic;

    public class Card {
        public readonly int Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string ActionsString;
        public readonly List<CardAction> Actions;
        public readonly object[] Properties;

        public Card(int pId, string pName, string pDescription, string pActions) {
            Id = pId;
            Name = pName;
            Description = pDescription;
            Actions = new List<CardAction>();

            ConvertActions(pActions);
        }

        public Card(int pId, string pName, string pDescription, List<CardAction> pActions) {
            Id = pId;
            Name = pName;
            Description = pDescription;
            Actions = pActions;
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

        public void Execute(Player pOwner, params object[] pProperties) {
            foreach (CardAction action in Actions) {
                action.Execute(pOwner, pProperties);
            }
        }
    }
}