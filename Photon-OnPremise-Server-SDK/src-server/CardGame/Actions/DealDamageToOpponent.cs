using System;
using System.Collections;
using System.Collections.Generic;

namespace CardGame {

    public class DealDamageToOpponent : CardAction {
        public const byte EVENT_CODE = 104;

        private int _amount = -5; // TODO: add the amount to the database so we can reuse actions, but change the values

        public override EventResponse Execute(PlayerState pPlayerState) {
            Player opponent = pPlayerState.Opponent;
            opponent.Health += _amount;

            int[] targets = {
                opponent.ActorNr
            };

            Hashtable data = new Hashtable();
            data.Add("code", EVENT_CODE);
            data.Add("targets", targets);
            data.Add("amount", _amount);

            return new EventResponse(EVENT_CODE, data);
        }
    }

}

