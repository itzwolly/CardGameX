public interface ITurnManagerCallbacks {
    void OnPlayerMove(PhotonPlayer pSender, int pCardId, int pCardIndex);

    void OnTurnBegins(int pTurn);

    void OnTurnEnds(int pTurn);
    
    void OnTurnTimeEnds(int pTurn);

    void OnNotYourTurn();
}
