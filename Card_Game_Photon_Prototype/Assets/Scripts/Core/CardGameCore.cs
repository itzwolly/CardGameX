using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

public class CardGameCore : PunBehaviour, IPunTurnManagerCallbacks {

    private TurnManager _turnManager;

    private void Start() {
        InitializeTurnManager();
        StartTurn();
    }

    private void InitializeTurnManager() {
        _turnManager = TurnManager.Instance;
        _turnManager.TurnManagerListener = this;
        _turnManager.TurnDuration = Config.TURN_DURATION;
    }

    private void StartTurn() {
        if (PhotonNetwork.player.IsMasterClient) {
            _turnManager.BeginTurn();
        }
    }

    private void Update() {
        Debug.Log("Turn: " + _turnManager.Turn + " Time Left: " + (int) (Config.TURN_DURATION - _turnManager.ElapsedTimeInTurn));

        if (Input.GetMouseButtonUp(0)) {
            PlayCard(559); // Example
        } else if (Input.GetMouseButtonUp(1)) {
            EndTurn(_turnManager.Turn);
        }
    }

    private void PlayCard(int pCardId) {
        // Do something
        Events.RaiseCardPlayedEvent(pCardId, PhotonNetwork.player.ID);
    }

    private void EndTurn(int pTurnId) {
        Events.RaiseEndTurnEvent(pTurnId);
    }

    public void OnPlayerFinished(PhotonPlayer pPlayer, int pTurn, object pMove) {
        // when a player made the last/final move in a turn
        throw new NotImplementedException();
    }

    public void OnPlayerMove(PhotonPlayer pPlayer, int pTurn, object pMove) {
        // when a player moved (but did not finish the turn)
        Debug.Log(pPlayer.ID + " has played card: " + pMove + " on turn: " + pTurn);
    }

    public void OnTurnBegins(int pTurn) {
        // Draw card???
        throw new NotImplementedException();
    }

    public void OnTurnCompleted(int pTurn) {
        StartTurn();
    }

    public void OnTurnTimeEnds(int pTurn) {
        // End turn
        throw new NotImplementedException();
    }
}
