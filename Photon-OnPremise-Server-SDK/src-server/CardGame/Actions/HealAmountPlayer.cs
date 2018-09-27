namespace CardGame {
    public class HealAmountPlayer : CardAction {

        public override void Execute(Player pOwner, params object[] pProperties) {
            //SerializableGameState gameState = (SerializableGameState) pProperties[0];
            pOwner.Health += 3; // normally would just use whatever is in pProperties
            //gameState.ActivePlayer.Health = pOwner.Health;
        }

    }

}

