using System;
using System.Collections.Generic;
using Photon.Hive.Plugin;
using static CardGame.PlayerState;
using System.Collections;

namespace CardGame.Events {
    public class AddTurboCardEvent : GameEvent {
        public const int EVENT_CODE = 114;

        private PlayerState _playerState = null;
        private Player _owner = null;

        // Constructor
        public AddTurboCardEvent(PlayerState pPlayerState) {
            _playerState = pPlayerState;
            _owner = _playerState.GetActivePlayer();
        }

        public override bool Handle(IRaiseEventCallInfo info, out List<EventResponse> pResponses) {
            if (info.UserId == _owner.UserId) {
                Hashtable data = (Hashtable) info.Request.Data;
                if (data.Count == 2) {
                    if (data.ContainsKey("cardid") && data.ContainsKey("cardindex")) {
                        if (_playerState.Data.ContainsKey((byte) PlayerStateKeys.TURBO_VIEWING_KEY) && _owner.Hand.Cards.Count < Hand.MAX_CARDS) {
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