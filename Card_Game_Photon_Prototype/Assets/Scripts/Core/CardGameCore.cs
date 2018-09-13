using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

public class CardGameCore : PunBehaviour, ITurnManagerCallbacks {
    [SerializeField] private HUDHandler _hud;
    private TurnManager _turnManager;

    private void Awake() {
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
        if (PhotonNetwork.player.IsMasterClient) {
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
        hand.DestroyCard(pCardIndex);

        // Execute whatever card: pCardId is..
    }

    public void OnTurnBegins(int pTurn) {
        // Draw card???
        Debug.Log("My turn just began. Can draw a card here...");
    }

    public void OnTurnEnds(int pTurn) {
        // start new turn
        _turnManager.BeginTurn();
    }

    public void OnTurnTimeEnds(int pTurn) {
        // start new turn
        _turnManager.BeginTurn();
    }

    public void OnNotYourTurn() {
        // Display error mesage
        _hud.DisplayEndTurnError(3);
    }

    public void OnGameWin() {
        Debug.Log("OnGameWin called baby..");
        //_hud.DisplayWinMessage();
    }
}
