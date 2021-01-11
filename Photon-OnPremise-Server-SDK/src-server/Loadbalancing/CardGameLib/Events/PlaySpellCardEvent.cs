using NLua;
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
                if (data.Count > 0 && data.Count <= 25) { // soft cap
                    if (data.ContainsKey("cardid") && data.ContainsKey("cardindex") && data.ContainsKey("targetamount")) { // need the card index for the CardPlayed (106) event
                        int cardId = (int) data["cardid"];

                        Card playedCard = _owner.Hand.GetCard(cardId);
                        if (playedCard != null && _owner.CurrentMana >= playedCard.RegCost) {
                            LuaTable table = Game.Instance.CreateTable(playedCard.GetBehaviour());

                            int targetAmount = (int) data["targetamount"];
                            for (int i = 0; i < targetAmount; i++) {
                                int id = (int) data["target_id-" + i];
                                int index = (int) data["target_index-" + i];
                                int ownerId = (int) data["target_ownerid-" + i];

                                if (_playerState.IsPlayerId(id)) {
                                    Player player = _playerState.GetPlayerById(id);
                                    if (player == null) {
                                        pResponses = null;
                                        return false;
                                    }

                                    table[i] = player as IInteractable;
                                } else {
                                    Player player = _playerState.GetPlayerById(ownerId);
                                    if (player == null) {
                                        pResponses = null;
                                        return false;
                                    }

                                    MonsterCard monster = player.BoardSide.GetMonsterByIndex(index);
                                    if (monster == null || monster.Id != id) {
                                        pResponses = null;
                                        return false;
                                    }

                                    table[i] = monster as IInteractable;
                                }
                            }

                            object[] results = playedCard.CallFunction("OnPlayed", (playedCard as IInteractable), table);
                            if (results[0] is bool) {
                                bool succeeded = (bool) results[0];

                                if (succeeded) {
                                    _owner.CurrentMana -= playedCard.RegCost;
                                    _owner.Hand.Cards.Remove(playedCard);

                                    pResponses = null;
                                    return true;
                                }
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

