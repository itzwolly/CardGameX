using System;
using System.Reflection;

namespace CardGame {

    public abstract class CardAction {
        public abstract void Execute(Player pOwner, params object[] pProperties);

        public static CardAction CreateInstance(string pFullyQualifiedName) {
            //dynamic cardActionsDll = Assembly.LoadFile("D:\\School\\Year 3\\Minor\\Card_Game_Repository\\Photon-OnPremise-Server-SDK\\src-server\\Loadbalancing\\CardGame.Actions\\bin\\Debug\\CardGame.Actions.dll");

            Type type = Type.GetType("CardGame." + pFullyQualifiedName);
            return (CardAction) Activator.CreateInstance(type);
        }
    }
}
