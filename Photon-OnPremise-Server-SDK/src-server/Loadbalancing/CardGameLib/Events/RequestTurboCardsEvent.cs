using System;
using System.Collections.Generic;
using Photon.Hive.Plugin;
using System.Collections;
using static CardGame.PlayerState;

namespace CardGame.Events {
    public class RequestTurboCardsEvent : GameEvent {
        public const int EVENT_CODE = 109;

        private PlayerState _playerState = null;
        private Player _owner = null;

        // Constructor
        public RequestTurboCardsEvent(PlayerState pPlayerState) {
            _playerState = pPlayerState;
            _owner = _playerState.GetActivePlayer();
        }

        public override bool Handle(IRaiseEventCallInfo info, out List<EventResponse> pResponses) {
            if (info.UserId == _owner.UserId) {
                if (!_playerState.Data.ContainsKey((byte) PlayerStateKeys.TURBO_VIEWING_KEY)) {
                    _playerState.Data.Add((byte) PlayerStateKeys.TURBO_VIEWING_KEY, _owner.ActorNr);

                    List<Card> turbo = _owner.Deck.GetTurboCards();
                    int turboCount = turbo.Count;

                    if (turboCount > 0) {
                        Hashtable data = new Hashtable();
                        data.Add("turbosize", turboCount);

                        for (int i = 0; i < turboCount; i++) {
                            Card card = turbo[i];
                            data.Add("id-" + i, card.Id);
                            data.Add("name-" + i, card.Name);
                            data.Add("type-" + i, card.GetCardType().Name);
                            data.Add("description-" + i, card.Description);
                            data.Add("turbocost-" + i, card.TurboCost);

                            if (card.IsMonster()) {
                                MonsterCard monster = card as MonsterCard;
                                data.Add("attack-" + i, monster.Attack);
                                data.Add("health-" + i, monster.Health);
                            }
                        }

                        List<EventResponse> responses = new List<EventResponse>();

                        EventResponse fullResponse = new EventResponse(110, data);
                        fullResponse.Receivers.Add(_owner.ActorNr);

                        EventResponse partialResponse = new EventResponse(110, turboCount);
                        partialResponse.Receivers.Add(_playerState.Opponent.ActorNr);

                        responses.Add(fullResponse);
                        responses.Add(partialResponse);

                        pResponses = responses;
                        return true;
                    }
                }
            }
            pResponses = null;
            return false;
        }
    }
}
