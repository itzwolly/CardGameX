using Photon.Hive.Plugin;

namespace CardGame.Events {
    public abstract class GameEvent {
        public abstract bool Handle(IRaiseEventCallInfo info);
    }

}
