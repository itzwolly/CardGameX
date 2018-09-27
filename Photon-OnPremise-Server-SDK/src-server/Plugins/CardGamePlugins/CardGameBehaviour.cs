using Photon.Hive.Plugin;
using Photon.Hive.Operations;
using CardGame;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using CardGame.Events;
using System.Collections;

namespace CardGamePlugins {

    public class CardGameBehaviour : PluginBase {
        public const string NAME = "CardGameBehaviour";
        private const int MAX_PLAYERS = 2;
        private const int TURN_TIME_IN_MS = 10000;

        private static Random _rnd = new Random();

        public override string Name {
            get {
                return NAME;
            }
        }

        private Player[] _players;

        public Player ActivePlayer {
            get;
            set;
        }
        public Player Opponent {
            get { return (ActivePlayer == _players[0] ? _players[1] : _players[0]); }
        }

        public override bool SetupInstance(IPluginHost host, Dictionary<string, string> config, out string errorMsg) {
            host.TryRegisterType(typeof(SerializableGameState), (byte) 'R', SerializeGameState, DeserializeGameState);

            return base.SetupInstance(host, config, out errorMsg);
        }

        public override void OnCreateGame(ICreateGameCallInfo info) {
            Deck deck = GetDeck("user_1" /*info.UserId*/);

            if (deck == null) {
                info.Fail("No (valid) deck can be found on the server. If you haven't made one, don't forget to do so first.");
                return;
            }

            _players = new Player[MAX_PLAYERS];
            _players[0] = new Player(1, info.UserId, deck);

            SerializableGameState gamestate = PluginHost.GetSerializableGameState();
            gamestate.Players = new SerializablePlayer[MAX_PLAYERS];
            gamestate.Players[0] = PlayerHelper.Serialize(_players[0]);
            SetGameState(gamestate);

            base.OnCreateGame(info);
        }

        public override void OnJoin(IJoinGameCallInfo info) {
            Deck deck = GetDeck("user_1" /*info.UserId*/);

            if (deck == null) {
                info.Fail("No (valid) deck can be found on the server. If you haven't made one, don't forget to do so first.");
                return;
            }

            _players[1] = new Player(info.ActorNr, info.UserId, deck);

            SerializableGameState gameState = PluginHost.GetSerializableGameState();
            gameState.Players[1] = PlayerHelper.Serialize(_players[1]);
            SetGameState(gameState);

            base.OnJoin(info);

            InitiateFullRoomSequence();
        }

        public override void OnRaiseEvent(IRaiseEventCallInfo info) {
            if (info.Request.EvCode == PlayCardEvent.EVENT_CODE) { // PLAY_CARD

                PlayCardEvent ev = new PlayCardEvent(ActivePlayer, Opponent);
                if (!ev.Handle(info)) {
                    info.Cancel();
                    return;
                }

                //SerializableGameState gameState = PluginHost.GetSerializableGameState();
                //gameState.ActivePlayer = PlayerHelper.Serialize(ActivePlayer);
                //gameState.Opponent = PlayerHelper.Serialize(Opponent);

                //gameState.ActivePlayer.Health = ActivePlayer.Health;
                //gameState.Opponent.Health = Opponent.Health;

                //SetGameState(gameState, true);

                //Hashtable data = new Hashtable();
                //data.Add("activeHealth", _players[0].Health);
                //data.Add("opponentHealth", _players[1].Health);
                
                Hashtable data = (Hashtable) info.Request.Parameters[245];
                int index = (int) data["cardindex"];

                info.Cancel();
                RaiseEvent(102, new int[] { ActivePlayer.Health, Opponent.Health, index }, ReciverGroup.All, ActivePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                return;
            }

            base.OnRaiseEvent(info); // process this event..
        }

        private void InitiateFullRoomSequence() {
            SerializableGameState gameState = PluginHost.GetSerializableGameState();
            List<int> receivers = new List<int>();

            for (int i = 0; i < gameState.ActorList.Count; i++) {
                receivers.Add(gameState.ActorList[i].ActorNr);
                RaiseEvent(99, "", receivers);
            }

            PluginHost.CreateTimer(SendStartTurnEvent, 100, TURN_TIME_IN_MS);
            PluginHost.CreateTimer(DrawCard, 200, TURN_TIME_IN_MS + 200);
        }

        private void SendStartTurnEvent() {
            SetActivePlayer();

            RaiseEvent(100, ActivePlayer.UserId, ReciverGroup.All, ActivePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
        }

        private void SetActivePlayer() {
            if (ActivePlayer == null) {
                ActivePlayer = _players[_rnd.Next(0, 2)]; // pick random player to start with
                return;
            }

            ActivePlayer = Opponent; // once the active player has been set, alternate between players...
        }

        private void DrawCard() {
            if (ActivePlayer.Hand == null) {
                ActivePlayer.Hand = new Hand();
            }

            Card drawn = ActivePlayer.Deck.DrawCard();
            ActivePlayer.Hand.Cards.Add(drawn);

            Hashtable data = new Hashtable();
            data.Add("id", drawn.Id);
            data.Add("name", drawn.Name);
            data.Add("description", drawn.Description);
            data.Add("handsize", ActivePlayer.Hand.Cards.Count);

            RaiseEvent(101, data, new int[] { ActivePlayer.ActorNr }); // sending a draw card event to the activeplayer, including the actual data.
            RaiseEvent(101, ActivePlayer.Hand.Cards.Count, new int[] { Opponent.ActorNr }); // sending a draw card event to the opponent, but without the actual data.
        }

        private Deck GetDeck(string pUsername) {
            Task<Deck> task = Task.Run(async () => {
                return await WebServer.GetDeckAsync(pUsername);
            });

            Deck deck = task.Result;
            if (deck != null) {
                return deck;
            }
            return null;
        }

        private void SetGameState(SerializableGameState pGameState, bool pBroadcast = false) {
            if (pBroadcast) {
                RaiseEvent(102, pGameState, ReciverGroup.All, pGameState.ActivePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache); // Update the players about this new gamestate..
            }

            PluginHost.SetGameState(pGameState); // Set gamestate
        }

        private byte[] SerializeGameState(object pObj) {
            SerializableGameState gameState = pObj as SerializableGameState;

            if (gameState == null) {
                return null;
            }

            using (var s = new MemoryStream()) {
                using (var bw = new BinaryWriter(s)) {
                    bw.Write(_players[0].Health);
                    bw.Write(_players[1].Health);
                    return s.ToArray();
                }
            }
        }

        private object DeserializeGameState(byte[] bytes) {
            SerializableGameState gameState = PluginHost.GetSerializableGameState();
            //using (var s = new MemoryStream(bytes)) {
            //    using (var br = new BinaryReader(s)) {
            //        //
            //    }
            //}
            return gameState;
        }
    }
}
