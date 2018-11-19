using System;
using System.Collections.Generic;

namespace CardGame {

    public class PlayerState {
        private static Random _rnd = new Random();

        private readonly int _maxPlayers;
        private readonly List<Player> _players;

        private Player _activePlayer = null;

        public Player Opponent {
            get { return (_activePlayer == _players[0] ? _players[1] : _players[0]); }
        }

        public PlayerState(int pMaxPlayers) {
            _maxPlayers = pMaxPlayers;
            _players = new List<Player>();
        }

        public Player AddPlayer(int pActorNr, string pUserId, Deck pDeck) {
            int count = _players.Count;
            if (count == _maxPlayers) {
                return null;
            }

            Player player = new Player(pActorNr, pUserId, pDeck);
            _players.Add(player);
            return player;
        }

        public Player SetActivePlayer() {
            if (_activePlayer == null) {
                _activePlayer = _players[_rnd.Next(0, 2)]; // pick random player to start with
                return _activePlayer;
            }
            _activePlayer = Opponent; // once the active player has been set, alternate between players...
            return _activePlayer;
        }

        public Player GetActivePlayer() {
            return _activePlayer;
        }
    }

}


