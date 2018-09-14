using UnityEngine;

/*
    Events will not be added to cache if any of the following conditions is met:
    RaiseEventOptions.Receivers == ReceiverGroups.MasterClient.
    RaiseEventOptions.TargetActors != null.
    RaiseEventOptions.InterestGroups != 0.
*/
public class Events {
    public const byte PLAY_CARD = 0;
    public const byte END_TURN = 1;
    public const byte JOIN_GAME = 2;
    public const byte END_GAME = 3;

    public static void RaiseCardPlayedEvent(int pCardId, int pCardIndex) {
        object[] content = new object[] { pCardId, pCardIndex };
        bool reliable = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache }; // You would have to set the Receivers to All in order to receive this event on the local client as well

        Debug.Log(raiseEventOptions.Receivers + " | " + raiseEventOptions.TargetActors + " | " + raiseEventOptions.InterestGroup);
        PhotonNetwork.RaiseEvent(PLAY_CARD, content, reliable, raiseEventOptions);
    }

    public static void RaiseEndTurnEvent(int pTurnId) {
        object[] content = new object[] { pTurnId };
        bool reliable = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCache }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(END_TURN, content, reliable, raiseEventOptions);
    }

    public static void RaiseJoinGameEvent(int pPlayerId) {
        object[] content = new object[] { pPlayerId };
        bool reliable = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(JOIN_GAME, content, reliable, raiseEventOptions);
    }

    public static void RaiseEndGameEvent() {
        bool reliable = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, TargetActors = new int[] { PhotonNetwork.player.ID, PhotonNetwork.otherPlayers[0].ID }, CachingOption = EventCaching.AddToRoomCache }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(END_GAME, null, reliable, raiseEventOptions);
    }
}
