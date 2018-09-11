public interface ITurnManagerCallbacks {
    void OnTurnBegins(int pTurn);

    void OnTurnEnds(int pTurn);

    void OnPlayerMove(PhotonPlayer pPlayer, int pCardId, int pCardIndex);
    
    void OnTurnTimeEnds(int pTurn);
}
