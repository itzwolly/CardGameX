using System;
using System.Collections.Generic;
using Photon.Hive.Plugin;
using static CardGame.PlayerState;
using System.Linq;

namespace CardGame.Events {
    public class RequestCloseTurboWindowEvent : GameEvent {
        public const int EVENT_CODE = 111;

        private PlayerState _playerState = null;
        private Player _owner = null;

        // Constructor
        public RequestCloseTurboWindowEvent(PlayerState pPlayerState) {
            _playerState = pPlayerState;
            _owner = _playerState.GetActivePlayer();
        }

        public override bool Handle(IRaiseEventCallInfo info, out List<EventResponse> pResponses) {
            if (info.UserId == _owner.UserId) {
                if (_playerState.Data.ContainsKey((byte) PlayerStateKeys.TURBO_VIEWING_KEY)) {
                    List<EventResponse> responses = new List<EventResponse>();
                    EventResponse response = new EventResponse(112, null);
                    response.Receivers.Add(_playerState.Opponent.ActorNr);

                    responses.Add(response);
                    pResponses = responses;

                    _playerState.Data.Remove((byte) PlayerStateKeys.TURBO_VIEWING_KEY);
                    return true;
                }
            }

            pResponses = null;
            return false;
        }

        /* Methods */
    }
}

