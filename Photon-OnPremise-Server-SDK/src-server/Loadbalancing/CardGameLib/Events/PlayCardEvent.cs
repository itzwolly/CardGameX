using Photon.Hive.Plugin;
using System.Collections;

//options.Receivers = ReceiverGroup.All;
//options.TargetActors = null;
//options.InterestGroup = 0;
//options.CachingOption = EventCaching.AddToRoomCache;
namespace CardGame.Events {
    public class PlayCardEvent : GameEvent {
        public const int EVENT_CODE = 102;
        private Player _owner;
        private object[] _properties;

        public PlayCardEvent(Player pOwner, params object[] pProperties) {
            _owner = pOwner;

            // ===== DEMO ======
            _properties = pProperties;
            //====== Demo ======
        }

        public override bool Handle(IRaiseEventCallInfo info) {
            if (info.UserId == _owner.UserId) { // check if its the correct persons turn before addressing card game specific events..
                Hashtable data = (Hashtable) info.Request.Data;
                if (data.Count == 2) {
                    if (data.ContainsKey("cardid") && data.ContainsKey("cardindex")) {
                        int cardId = (int) data["cardid"];
                        int cardIndex = (int) data["cardindex"];
                        
                        // TODO: The card should contain properties e.g: wether or not the target is
                        // a minion or enemy etc. and this can be given to the action through the properties
                        // params. This data will be saved in the card itself, wether it be the database or json format.

                        Card playedCard = _owner.Hand.GetCard(cardId);
                        if (playedCard != null) {
                            playedCard.Execute(_owner, _properties);
                            _owner.Hand.Cards.Remove(playedCard);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}

