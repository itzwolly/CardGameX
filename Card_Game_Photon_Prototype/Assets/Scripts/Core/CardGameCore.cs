using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CardGameCore : PunBehaviour, ITurnManagerCallbacks {

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

            _turnManager.BeginTurn();
        }
    }

    public void EndTurn() {
        _turnManager.EndTurn();
    }

    public void OnPlayerMove(PhotonPlayer pPlayer, int pCardId, int pCardIndex) {
        // when a player made a move
        Debug.Log(pPlayer.ID + " played card: " + pCardId + " | Card index: " + pCardIndex);

        HandManager hand = PhotonNetwork.player.TagObject as HandManager;
        hand.DestroyCard(pCardIndex);

        // Execute whatever card: pCardId is..

    }

    public void OnTurnBegins(int pTurn) {
        // Draw card???
        throw new NotImplementedException();
    }

    public void OnTurnEnds(int pTurn) {
        // start new turn
        _turnManager.BeginTurn();
        //throw new NotImplementedException();
    }

    public void OnTurnTimeEnds(int pTurn) {
        // start new turn
        _turnManager.BeginTurn();
        //throw new NotImplementedException();
    }
}
