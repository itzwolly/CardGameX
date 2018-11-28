using Photon.Hive.Plugin;
using System.Collections;
using System.Collections.Generic;

namespace CardGame.Events {
    public class PlaySpellCardEvent : GameEvent {
        public const int EVENT_CODE = 103;

        private PlayerState _playerState = null;
        private Player _owner = null;

        public PlaySpellCardEvent(PlayerState pPlayerState) {
            _playerState = pPlayerState;
            _owner = _playerState.GetActivePlayer();
        }

        public override bool Handle(IRaiseEventCallInfo info, out List<EventResponse> pResponses) {
            if (info.UserId == _owner.UserId) { // check if its the correct persons turn before addressing card game specific events..
                Hashtable data = (Hashtable) info.Request.Data;
                if (data.Count == 2) { 
                    if (data.ContainsKey("cardid") && data.ContainsKey("cardindex")) { // need the card index for the CardPlayed (106) event
                        int cardId = (int) data["cardid"];

                        Card playedCard = _owner.Hand.GetCard(cardId);
                        if (playedCard != null && _owner.CurrentMana >= playedCard.RegCost) {
                            //List<EventResponse> responses = playedCard.Execute(_playerState);
                            
                            _owner.CurrentMana -= playedCard.RegCost;
                            _owner.Hand.Cards.Remove(playedCard);

                            pResponses = null;
                            return true;
                        }
                    }
                }
            }
            pResponses = null;
            return false;
        }
    }
}

