public interface ITurnManagerCallbacks {
    void OnPlayerMove(PhotonPlayer pSender, int pCardId, int pCardIndex);
    void OnTurnBegins(PhotonPlayer pSender);
    void OnTurnEnd(PhotonPlayer pSender);
    void OnTurnTimeEnds(PhotonPlayer pSender);
    void OnNotYourTurn(PhotonPlayer pSender);
    void OnGameEnd(PhotonPlayer pWinner);
    void OnCardDrawn(PhotonPlayer pSender, int pHandSize);
}
