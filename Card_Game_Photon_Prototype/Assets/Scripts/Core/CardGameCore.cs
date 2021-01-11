using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

    public Player Opponent {
        get { return (_activePlayer == _players[0] ? _players[1] : _players[0]); }
    }
    public bool SelectingSpellTarget {
        get;
        set;
    }

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

    public void UpdatePlayerResources(Player pPlayer) {
        if (pPlayer.UserId == PhotonNetwork.player.UserId) {
            _hud.PlayerMana.text = "Mana: " + pPlayer.CurrentMana.ToString() + "/" + pPlayer.TotalMana.ToString();
            _hud.PlayerTurbo.text = "Turbo: " + pPlayer.CurrentTurbo.ToString() + "/" + pPlayer.TotalTurbo.ToString();
            _hud.PlayerHealth.text = pPlayer.Health.ToString();
        } else {
            _hud.EnemyMana.text = "Mana: " + pPlayer.CurrentMana.ToString() + "/" + pPlayer.TotalMana.ToString();
            _hud.EnemyTurbo.text = "Turbo: " + pPlayer.CurrentTurbo.ToString() + "/" + pPlayer.TotalTurbo.ToString();
            _hud.EnemyHealth.text = pPlayer.Health.ToString();
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

    public void Interpret(Enhancement pEnhancement) {
        BoardEnhancements enhancement = pEnhancement.BoardEnhancement;

        switch (enhancement) {
            case BoardEnhancements.Set_Health: {
                    Player target = GetPlayerByActorNr(pEnhancement.Id);
                    int value = pEnhancement.Value;

                    if (target != null) {
                        target.Health = value;
                        UpdatePlayerResources(target);
                    } else {
                        int targetIndex = pEnhancement.BoardIndex;
                        int targetOwnerId = pEnhancement.OwnerId;

                        if (pEnhancement.OwnerId != PhotonNetwork.player.ID) {
                            targetIndex = Mathf.Abs(targetIndex - (BoardManager.Instance.GetBoardById(targetOwnerId).childCount - 1));
                        }

                        BoardCardBehaviour bcb = BoardManager.Instance.GetBoardCardBehaviour(targetOwnerId, targetIndex);
                            MonsterCardData data = bcb.Data;
                            data.Health = value;
                        bcb.Data = data;

                        Debug.Log("Target: " + data.Name + " health equals to " + data.Health);
                    }
                    break;
                }
            case BoardEnhancements.Set_Attack: {
                    Player target = GetPlayerByActorNr(pEnhancement.Id);
                    int value = pEnhancement.Value;

                    if (target != null) {
                        // TODO: Implement player hero attack.
                    } else {
                        int targetIndex = pEnhancement.BoardIndex;
                        int targetOwnerId = pEnhancement.OwnerId;

                        if (pEnhancement.OwnerId != PhotonNetwork.player.ID) {
                            targetIndex = Mathf.Abs(targetIndex - (BoardManager.Instance.GetBoardById(targetOwnerId).childCount - 1));
                        }

                        BoardCardBehaviour bcb = BoardManager.Instance.GetBoardCardBehaviour(targetOwnerId, targetIndex);
                            MonsterCardData data = bcb.Data;
                            data.Attack = value;
                        bcb.Data = data;
                    }
                    break;
                }
            case BoardEnhancements.Add_Health: {
                    Player target = GetPlayerByActorNr(pEnhancement.Id);
                    int value = pEnhancement.Value;

                    Debug.Log("Adding the following health: " + value);

                    if (target != null) {
                        target.Health += value;
                        UpdatePlayerResources(target);
                    } else {
                        int targetIndex = pEnhancement.BoardIndex;
                        int targetOwnerId = pEnhancement.OwnerId;

                        if (pEnhancement.OwnerId != PhotonNetwork.player.ID) {
                            targetIndex = Mathf.Abs(targetIndex - (BoardManager.Instance.GetBoardById(targetOwnerId).childCount - 1));
                        }

                        BoardCardBehaviour bcb = BoardManager.Instance.GetBoardCardBehaviour(targetOwnerId, targetIndex);
                            MonsterCardData data = bcb.Data;
                            data.Health += value;
                        bcb.Data = data;

                        Debug.Log("Target: " + data.Name + " health equals to " + data.Health);
                    }
                    break;
                }
            case BoardEnhancements.Add_Attack: {
                    Player target = GetPlayerByActorNr(pEnhancement.Id);
                    int value = pEnhancement.Value;

                    Debug.Log("Adding the following attack: " + value);

                    if (target != null) {
                        // TODO: Implement player hero attack.
                    } else {
                        int targetIndex = pEnhancement.BoardIndex;
                        int targetOwnerId = pEnhancement.OwnerId;

                        if (pEnhancement.OwnerId != PhotonNetwork.player.ID) {
                            targetIndex = Mathf.Abs(targetIndex - (BoardManager.Instance.GetBoardById(targetOwnerId).childCount - 1));
                        }

                        BoardCardBehaviour bcb = BoardManager.Instance.GetBoardCardBehaviour(targetOwnerId, targetIndex);
                            MonsterCardData data = bcb.Data;
                            data.Attack += value;
                        bcb.Data = data;

                        Debug.Log("Target: " + data.Name + " attack equals to " + data.Attack);
                    }
                    break;
                }
            case BoardEnhancements.Can_Attack:
            case BoardEnhancements.Has_Single_Shield:
            case BoardEnhancements.None:
            default:
                break;
        }
    }

    public enum BoardEnhancements {
        // Termination
        None,
        // Basics
        Set_Health,
        Set_Attack,
        Add_Health,
        Add_Attack,
        // Rush
        Can_Attack,
        // Shield up
        Has_Single_Shield,
        Spell_Taunt,
    }

    public enum SpellTargetType {
        None,
        Enemy_Monster,
        Enemy_Hero,
        Enemy_Any,
        Player_Monster,
        Player_Hero,
        Player_Any
    }
}
