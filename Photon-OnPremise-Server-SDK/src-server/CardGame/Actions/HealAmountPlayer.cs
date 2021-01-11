

using System.Collections;
using System.Collections.Generic;

namespace CardGame {

    public class HealAmountPlayer : CardAction {
        public const byte EVENT_CODE = 104;

        private int _amountToIncrease = 3; // TODO: add the amount to the database so we can reuse actions, but change the values
        
        public override EventResponse Execute(PlayerState pPlayerState) {
            Player player = pPlayerState.GetActivePlayer();
            player.Health += _amountToIncrease;

            int[] targets = {
                player.ActorNr
            };

            Hashtable data = new Hashtable();
            data.Add("targets", targets);
            data.Add("amount", _amountToIncrease);

            return new EventResponse(EVENT_CODE, data);
        }
    }

}

