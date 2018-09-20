public interface ITurnManagerCallbacks {
    void OnPlayerMove(PhotonPlayer pSender, int pCardId, int pCardIndex);

    void OnTurnBegins();

    void OnTurnEnds();
    
    void OnTurnTimeEnds();

    void OnNotYourTurn();

    void OnGameEnd(PhotonPlayer pWinner);

    void OnCardDrawn(PhotonPlayer pSender, int pHandSize);
}
