using System;
using System.Collections.Generic;

public class Player {
    private const int MAX_MANA = 10;
    private const int MAX_TURBO = 5;

    public int Health;
    private int _totalMana;
    private int _totalTurbo;
    private int _currentMana;
    private int _currentTurbo;

    public readonly int ActorNr;
    public readonly string UserId;

    
    public int CurrentMana {
        get { return _currentMana; }
        set {
            if (_currentMana >= MAX_MANA) {
                return;
            }
            _currentMana = value;
        }
    }
    public int CurrentTurbo {
        get { return _currentTurbo; }
        set {
            if (_currentTurbo >= MAX_TURBO) {
                return;
            }
            _currentTurbo = value;
        }
    }
    public int TotalMana {
        get { return _totalMana; }
        set {
            if (_totalMana >= MAX_MANA) {
                _currentMana = MAX_MANA;
                return;
            }
            _totalMana = value;
            _currentMana = _totalMana;
        }
    }
    public int TotalTurbo {
        get { return _totalTurbo; }
        set {
            if (_totalTurbo >= MAX_TURBO) {
                _currentTurbo = MAX_MANA;
                return;
            }
            _totalTurbo = value;
            _currentTurbo = _totalTurbo;
        }
    }

    public Player(int pActorNr, string pUserId, int pStartingHealth) {
        ActorNr = pActorNr;
        UserId = pUserId;
        Health = pStartingHealth;
    }
}
