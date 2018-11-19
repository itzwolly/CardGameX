using Photon.Hive.Plugin;
using System.Collections;
using System.Collections.Generic;

namespace CardGame.Events {
    public class PlayMonsterCardEvent : GameEvent {
        public const int EVENT_CODE = 105;

        private BoardState _boardState = null;
        private Player _owner = null;

        public PlayMonsterCardEvent(BoardState pBoardState) {
            _boardState = pBoardState;
            _owner = _boardState.PlayerState.GetActivePlayer();
        }

        public override bool Handle(IRaiseEventCallInfo info, out List<EventResponse> pResponses) {
            if (info.UserId == _owner.UserId) { // check if its the correct persons turn before addressing card game specific events..
                Hashtable data = (Hashtable) info.Request.Data;
                if (data.Count == 3) {
                    if (data.ContainsKey("cardid") && data.ContainsKey("cardindex") && data.ContainsKey("boardindex")) {
                        int cardId = (int) data["cardid"];
                        int boardIndex = (int) data["boardindex"];

                        MonsterCard playedCard = _owner.Hand.GetCard(cardId) as MonsterCard;
                        if (playedCard != null && _owner.CurrentMana >= playedCard.RegCost) {
                            if (_owner.BoardSide.Occupy(boardIndex, cardId)) {
                                data.Add("name", playedCard.Name);
                                data.Add("description", playedCard.Description);
                                data.Add("regcost", playedCard.RegCost);
                                data.Add("turbocost", playedCard.TurboCost);
                                data.Add("attack", playedCard.Attack);
                                data.Add("health", playedCard.Health);

                                List<EventResponse> responses = new List<EventResponse>();
                                responses.Add(new EventResponse(107, data));
                                pResponses = responses;

                                _owner.CurrentMana -= playedCard.RegCost;
                                _owner.Hand.Cards.Remove(playedCard);
                                return true;
                            }
                        }
                    }
                }
            }
            pResponses = null;
            return false;
        }
    }

}
