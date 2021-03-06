﻿public class Config  {
    public const string VERSION = "v0.02";
    public const string GAME_SCENE = "GameScene";
    public const string MAIN_SCENE = "Networking";
    public const string CARD_COLLECTION_SCENE = "CardCollection";
    public const string PLAYER_PREFAB = "Player";
    public static string[] PLUGINS_NAME = new string[] { "CardGameBehaviour" };
    public const int MAX_PLAYERS = 2;
    public const int PLAYER_TTL = 20000;
    public const int EMPTY_ROOM_TTL = 0; // Possibly change to 3000, if there is issues with rooms not existing or w.e
    public const float TURN_DURATION = 60f;
    public const bool CLEANUP_CACHE_ON_LEAVE = false;
    public const int MAX_CARDS_PER_PAGE = 10;
    public const int DECK_REGULAR_SIZE = 20;
    public const int DECK_TURBO_SIZE = 5;
    public const int DECK_REG_CARD_LIMIT = 3;
    public const int MAX_DECK_SIZE = 6;
    public const int DECK_TURBO_CARD_LIMIT = 1;
    public const int MAX_HAND_SIZE = 5;
    public const int START_CARD_AMOUNT = 2;
}
