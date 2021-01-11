using Photon.Hive.Plugin;
using System.Collections.Generic;

namespace CardGame.Events {
    public abstract class GameEvent {
        public abstract bool Handle(IRaiseEventCallInfo info, out List<EventResponse> pResponses);
    }

}
