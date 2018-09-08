public class Events {
    public const byte PLAY_CARD = 0;

    public static void RaiseCardPlayedEvent(int pSlotIndex, int pPlayerIndex) {
        object[] content = new object[] { pSlotIndex, pPlayerIndex }; // Array contains which board slot was selected
        bool reliable = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(PLAY_CARD, content, reliable, raiseEventOptions);
    }
}
