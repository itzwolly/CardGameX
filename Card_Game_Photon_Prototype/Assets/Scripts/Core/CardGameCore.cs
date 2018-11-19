using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Timers;
using System;
using System.Collections;

public class CardGameCore : MonoBehaviour {
    #region Singleton
    private static CardGameCore _instance;
    public static CardGameCore Instance {
        get {
            if (!_instance) {
                _instance = FindObjectOfType(typeof(CardGameCore)) as CardGameCore;

                if (!_instance) {
                    Debug.Log("There should be an active object with the component CardGameCore");
                } else {
                    _instance.Init();
                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private GameHUDHandler _hud;

    private List<Player> _players = null;
    private Player _activePlayer = null;

    private Coroutine _turnTimer;

    public void Init() {
        _players = new List<Player>();
    }

    public void AddPlayer(int pActorId, string pUserId, int pStartingHealth) {
        Player player = new Player(pActorId, pUserId, pStartingHealth);

        Debug.Log("Adding player...");
        _players.Add(player);
    }

    public void SetActivePlayer(Player pPlayer) {
        _activePlayer = pPlayer;
        _hud.ActivePlayerText.text = _activePlayer.UserId;
    }

    public void UpdateResources(Player pPlayer) {
        if (pPlayer.UserId == PhotonNetwork.player.UserId) {
            _hud.PlayerMana.text = "Mana: " + pPlayer.CurrentMana.ToString() + "/" + pPlayer.TotalMana.ToString();
            _hud.PlayerTurbo.text = "Turbo: " + pPlayer.CurrentTurbo.ToString() + "/" + pPlayer.TotalTurbo.ToString();
        } else {
            _hud.EnemyMana.text = "Mana: " + pPlayer.CurrentMana.ToString() + "/" + pPlayer.TotalMana.ToString();
            _hud.EnemyTurbo.text = "Turbo: " + pPlayer.CurrentTurbo.ToString() + "/" + pPlayer.TotalTurbo.ToString();
        }
    }

    public void UpdatePlayerHealth(Player pPlayer, int pAmount) {
        pPlayer.Health += pAmount;

        int id = pPlayer.ActorNr;
        if (id == PhotonNetwork.player.ID) {
            _hud.PlayerHealth.text = pPlayer.Health.ToString();
        } else {
            _hud.EnemyHealth.text = pPlayer.Health.ToString();
        }
    }

    public void StartTurnTimer(int pTurnTimeLimit) {
        if (_turnTimer != null) {
            StopCoroutine(_turnTimer);
        }
        _turnTimer = StartCoroutine(StartTimer(pTurnTimeLimit));
    }

    private IEnumerator StartTimer(int pStartTime) {
        int counter = pStartTime;
        _hud.UpdateTurnTimer(counter);

        while (counter >= 0) {
            yield return new WaitForSeconds(1);
            counter--;
            _hud.UpdateTurnTimer(counter);
        }
    }

    public Player GetActivePlayer() {
        return _activePlayer;
    }

    public Player GetPlayerByUserId(string pUserId) {
        return _players.FirstOrDefault(o => o.UserId == pUserId);
    }

    public Player GetPlayerByActorNr(int pActorNr) {
        return _players.FirstOrDefault(o => o.ActorNr == pActorNr);
    }
}
