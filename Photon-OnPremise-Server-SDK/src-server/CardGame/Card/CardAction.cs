using System;

namespace CardGame {

    public abstract class CardAction {
        public abstract EventResponse Execute(PlayerState pPlayerState);

        public static CardAction CreateInstance(string pFullyQualifiedName) {
            Type type = Type.GetType("CardGame." + pFullyQualifiedName);
            return (CardAction) Activator.CreateInstance(type);
        }
    }
}
