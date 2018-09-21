using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

public class CardGameCore : PunBehaviour, ITurnManagerCallbacks {
    [SerializeField]
    private GameHUDHandler _hud;

    private TurnManager _turnManager;

    private void Awake() {
        Debug.Log("Initializing Turn manager in the Game's core.");
        InitializeTurnManager();
    }

    private void Start() {
        StartGame();
    }

    private void InitializeTurnManager() {
        _turnManager = TurnManager.Instance;
        _turnManager.TurnManagerListener = this;
        _turnManager.TurnDuration = Config.TURN_DURATION;
    }

    private void StartGame() {
        if (PhotonNetwork.player.IsMasterClient && _turnManager.CurrentTurn == 0) {
            Debug.Log("Calling Start in the Game's Core..");
            Debug.Log("Current master client is: " + PhotonNetwork.player.ID);
            _turnManager.StartGame();
        }
    }

    public void OnPlayerMove(PhotonPlayer pPlayer, int pCardId, int pCardIndex) {
        // when a player made a move
        Debug.Log(pPlayer.ID + " played card: " + pCardId + " | Card index: " + pCardIndex);

        HandManager handManager = PhotonNetwork.player.TagObject as HandManager;
        handManager.DestroyCard(pPlayer.ID, pCardIndex);

        // Execute whatever card: pCardId is..
        Hand.Instance.PlayCard(pCardId);
    }

    public void OnTurnBegins(PhotonPlayer pSender) {
        if (_turnManager.IsActivePlayer && pSender.UserId == PhotonNetwork.player.UserId) { // Check if whoever recieved this event is the one who send it (aka the player)
            if (Hand.Instance.GetCards().Count >= Config.HAND_SIZE) {
                Debug.Log("You've exceeded the limit of the amount of cards in your hand.");
                return;
            }

            Card drawn = DeckHandler.Instance.ActiveDeck.DrawCard();
            if (drawn != null) {
                Debug.Log("My turn just began. Drew card: " + drawn.Data.Name);
                Hand.Instance.AddCard(drawn);
            } else {
                Debug.Log("Sorry pal.. Your deck is empty.");
            }
        }
        
        Events.RaiseCardDrawnEvent(Hand.Instance.GetCards().Count);
    }

    public void OnCardDrawn(PhotonPlayer pSender, int pHandSize) {
        if (!_turnManager.IsActivePlayer && pSender.UserId != PhotonNetwork.player.UserId) { // Check if whoever recieved this event is not the one who send it (aka the enemy)
            Debug.Log("Handsize inside display enemy card: " + pHandSize);
            HandManager manager = PhotonNetwork.player.TagObject as HandManager;
            manager.DisplayEnemyCardDrawn(pHandSize);
        }
    }

    public void OnTurnEnd(PhotonPlayer pPlayer) {
        // start new turn
        Debug.Log("Calling OnTurnEnds" + PhotonNetwork.player.ID);
        _turnManager.BeginTurn();
    }

    public void OnTurnTimeEnds(PhotonPlayer pPlayer) {
        // start new turn
        Debug.Log("Calling OnTurnTimeEnds" + PhotonNetwork.player.ID);
        _turnManager.BeginTurn();
    }

    public void OnNotYourTurn(PhotonPlayer pPlayer) {
        // Display error mesage
        _hud.DisplayEndTurnError(3);
    }

    public void OnGameEnd(PhotonPlayer pWinner) {
        Debug.Log("OnGameEnd called.");

        if (pWinner.ID == PhotonNetwork.player.ID) {
            Debug.Log("Displaying win message and changing back to main scene after 3 seconds.");
            _hud.EngageWinSequence(3);
        } else if (pWinner.ID == PhotonNetwork.otherPlayers[0].ID) {
            Debug.Log("Displaying loss message and changing back to main scene after 3 seconds..");
            _hud.EngageLoseSequence(5);
        }
    }
}
