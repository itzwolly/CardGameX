using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

public class CardGameCore : PunBehaviour, ITurnManagerCallbacks {
    [SerializeField] private GameHUDHandler _hud;

    private TurnManager _turnManager;

    private void Awake() {
        Debug.Log("Initializing Turn manager in the Game's core.");
        InitializeTurnManager();
    }

    private void Start() {
        Debug.Log("Calling Start in the Game's Core..");

        StartGame();
    }

    private void InitializeTurnManager() {
        _turnManager = TurnManager.Instance;
        _turnManager.TurnManagerListener = this;
        _turnManager.TurnDuration = Config.TURN_DURATION;
    }

    private void StartGame() {
        if (PhotonNetwork.player.IsMasterClient && _turnManager.CurrentTurn == 0) {
            Debug.Log("Current master client is: " + PhotonNetwork.player.ID);
            _turnManager.StartGame();
        }
    }

    public void EndTurn() {
        // Raises an onTurnEnds event.
        if (!_turnManager.EndTurn()) {
            // Display message that it's not your turn..
            _turnManager.TurnManagerListener.OnNotYourTurn();
        }
    }

    public void OnPlayerMove(PhotonPlayer pPlayer, int pCardId, int pCardIndex) {
        // when a player made a move
        Debug.Log(pPlayer.ID + " played card: " + pCardId + " | Card index: " + pCardIndex);

        HandManager hand = PhotonNetwork.player.TagObject as HandManager;
        // Would normally just play a certain animation..
        // Just destroying for now.
        hand.DestroyCard(pPlayer.ID, pCardIndex);

        // Execute whatever card: pCardId is..
        
    }

    public void OnTurnBegins(int pTurn) {
        // Draw card???
        Card drawnCard = DeckHandler.Instance.ActiveDeck.DrawCard();

        if (drawnCard != null) {
            Debug.Log("My turn just began. Drew card: " + drawnCard.Data.Name);
        } else {
            Debug.Log("Sorry pal.. Your deck is empty.");
        }
    }

    public void OnTurnEnds(int pTurn) {
        // start new turn
        Debug.Log("Calling OnTurnEnds");
        _turnManager.BeginTurn();
    }

    public void OnTurnTimeEnds(int pTurn) {
        // start new turn
        Debug.Log("Calling OnTurnTimeEnds");
        _turnManager.BeginTurn();
    }

    public void OnNotYourTurn() {
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
