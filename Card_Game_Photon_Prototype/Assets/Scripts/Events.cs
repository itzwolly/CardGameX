using UnityEngine;

public class Events {
    public const byte PLAY_CARD = 0;
    public const byte END_TURN = 1;
    public const byte JOIN_GAME = 2;

    public static void RaiseCardPlayedEvent(int pCardId, int pCardIndex) {
        object[] content = new object[] { pCardId, pCardIndex };
        bool reliable = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(PLAY_CARD, content, reliable, raiseEventOptions);
    }

    public static void RaiseEndTurnEvent(int pTurnIndex) {
        object[] content = new object[] { pTurnIndex };
        bool reliable = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(END_TURN, content, reliable, raiseEventOptions);
    }

    public static void RaiseJoinGameEvent(int pPlayerIndex) {
        object[] content = new object[] { pPlayerIndex };
        bool reliable = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(JOIN_GAME, content, reliable, raiseEventOptions);
    }
}
