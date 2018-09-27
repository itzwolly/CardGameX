using System;
using System.Collections.Generic;

namespace CardGame {

    public class DealDamageToOpponent : CardAction {
        public override void Execute(Player pOwner, params object[] pProperties) {
            //SerializableGameState gameState = (SerializableGameState) pProperties[0];
            Player opponent = (Player) pProperties[0];

            opponent.Health -= 5; // again normally the propeties variable will have more data than just the target in this case.
            //gameState.Opponent.Health = opponent.Health;
        }

    }

}

