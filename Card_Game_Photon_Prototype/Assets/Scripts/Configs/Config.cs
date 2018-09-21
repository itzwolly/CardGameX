public class Config  {
    public const string VERSION = "v0.02";
    public const string GAME_SCENE = "GameScene";
    public const string MAIN_SCENE = "Networking";
    public const string CARD_COLLECTION_SCENE = "CardCollection";
    public const string PLAYER_PREFAB = "Player";
    public const int MAX_PLAYERS = 2;
    public const int PLAYER_TTL = 20000;
    public const int EMPTY_ROOM_TTL = 0; // Possibly change to 3000, if there is issues with rooms not existing or w.e
    public const float TURN_DURATION = 60f;
    public const bool CLEANUP_CACHE_ON_LEAVE = false;
    public const int MAX_CARDS_PER_PAGE = 10;
    public const int DECK_SIZE = 20;
    public const int CARD_LIMIT = 3;
    public const int HAND_SIZE = 5;
    public const int START_CARD_AMOUNT = 2;
}
