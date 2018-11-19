using Photon.Hive.Plugin;
using Photon.Hive.Operations;
using CardGame;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using CardGame.Events;
using System.Collections;
using System.Threading;

namespace CardGamePlugins {

    public class CardGameBehaviour : PluginBase {
        public const string NAME = "CardGameBehaviour";
        private const int MAX_PLAYERS = 2;
        private const int TURN_TIME_IN_MS = 10000; // 60000

        public override string Name {
            get { return NAME; }
        }

        private Game _game = null;

        public override void OnCreateGame(ICreateGameCallInfo info) {
            string deckCode = info.OperationRequest.Parameters[189] as string;
            string turboCode = info.OperationRequest.Parameters[190] as string;
            Deck deck = GetDeck(deckCode, turboCode);

            if (deck == null) {
                info.Fail("No (valid) deck can be found on the server. If you haven't made one, don't forget to do so first.");
                return;
            }

            _game = new Game(MAX_PLAYERS);
            _game.AddPlayer(1, info.UserId, deck);

            base.OnCreateGame(info);

            //InitiateFullRoomSequence();
        }

        public override void OnJoin(IJoinGameCallInfo info) {
            string deckCode = info.OperationRequest.Parameters[189] as string;
            string turboCode = info.OperationRequest.Parameters[190] as string;
            Deck deck = GetDeck(deckCode, turboCode);

            if (deck == null) {
                info.Fail("No (valid) deck can be found on the server. If you haven't made one, don't forget to do so first.");
                return;
            }

            _game.AddPlayer(info.ActorNr, info.UserId, deck);

            base.OnJoin(info);

            InitiateFullRoomSequence();
        }

        public override void OnRaiseEvent(IRaiseEventCallInfo info) {
            if (info.Request.EvCode == PlaySpellCardEvent.EVENT_CODE) {
                PlayerState state = _game.PlayerState;
                PlaySpellCardEvent playSCEvent = new PlaySpellCardEvent(state);

                List<EventResponse> responses;
                bool handled = playSCEvent.Handle(info, out responses);
                if (handled) {
                    Player activePlayer = state.GetActivePlayer();

                    // raise card played event.. //
                    Hashtable data = (Hashtable) info.Request.Data;
                    data.Add("mana", activePlayer.CurrentMana);

                    int cardId = (int) data["cardid"];
                    int index = (int) data["cardindex"];
                    
                    RaiseEvent(106, data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    // end raise card played event //

                    foreach (EventResponse response in responses) {
                        RaiseEvent(response.EventCode, response.Data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    }
                }
                info.Cancel();
                return;
            } else if (info.Request.EvCode == PlayMonsterCardEvent.EVENT_CODE) {
                BoardState state = _game.BoardState;
                PlayMonsterCardEvent playMCEvent = new PlayMonsterCardEvent(state);

                List<EventResponse> responses;
                bool handled = playMCEvent.Handle(info, out responses);
                if (handled) {
                    Player activePlayer = state.PlayerState.GetActivePlayer();

                    // raise card played event.. //
                    Hashtable data = (Hashtable) info.Request.Data;
                    data.Add("mana", activePlayer.CurrentMana);
                    data.Add("totalmana", activePlayer.TotalMana);

                    int cardId = (int) data["cardid"];
                    int index = (int) data["cardindex"];

                    RaiseEvent(106, data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    // end raise card played event //

                    foreach (EventResponse response in responses) {
                        RaiseEvent(response.EventCode, response.Data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    }
                }
                info.Cancel();
                return;
            } else if (info.Request.EvCode == 98) { // LoadedGameplayScene.EVENT_CODE
                if (!_game.Started) {
                    PluginHost.CreateTimer(SendStartTurnEvent, 200, TURN_TIME_IN_MS);
                    PluginHost.CreateTimer(DrawCard, 300, TURN_TIME_IN_MS + 300); // small delay.. as to not spam the client with events

                    _game.Started = true;

                    info.Cancel();
                    return;
                }
            }

            // TODO: only process the neccessary events a.k.a: ignore out of context events raised by the client.
            base.OnRaiseEvent(info); // process this event.. 
        }

        private void InitiateFullRoomSequence() {
            SerializableGameState gameState = PluginHost.GetSerializableGameState();
            int[] receivers = new int[gameState.ActorList.Count];
            string[] userIds = new string[gameState.ActorList.Count];

            for (int i = 0; i < gameState.ActorList.Count; i++) {
                SerializableActor actor = gameState.ActorList[i];
                receivers[i] = actor.ActorNr;
                userIds[i] = actor.UserId;
            }

            Hashtable data = new Hashtable();
            data.Add("playeractorids", receivers);
            data.Add("playeruserids", userIds);
            data.Add("startinghealth", Player.STARTING_HEALTH);
            
            RaiseEvent(99, data, receivers);
        }

        private void SendStartTurnEvent() {
            // TODO: Before starting new turn, check if there are any events waiting to be executed.
            Player activePlayer = _game.PlayerState.SetActivePlayer(); 
            activePlayer.TotalMana++;
            activePlayer.TotalTurbo++;

            Hashtable data = new Hashtable();
            data.Add("userid", activePlayer.UserId);
            data.Add("mana", 1);
            data.Add("turbo", 1);
            data.Add("turntimelimit", TURN_TIME_IN_MS / 1000);
            
            RaiseEvent(100, data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
        }

        private void DrawCard() {
            Player activePlayer = _game.PlayerState.GetActivePlayer();
            Card drawn = activePlayer.Deck.DrawCard();

            Hashtable data = new Hashtable();
            data.Add("cardid", drawn.Id);
            data.Add("name", drawn.Name);
            data.Add("type", drawn.GetCardType().Name);
            data.Add("regcost", drawn.RegCost);
            data.Add("description", drawn.Description);

            if (drawn.IsMonster()) {
                MonsterCard monster = drawn as MonsterCard;
                data.Add("attack", monster.Attack);
                data.Add("health", monster.Health);
            }

            if (activePlayer.Hand.Cards.Count >= Hand.MAX_CARDS) {
                RaiseEvent(102, data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                return;
            }

            activePlayer.Hand.Cards.Add(drawn);
            data.Add("handsize", activePlayer.Hand.Cards.Count);

            RaiseEvent(101, data, new int[] { activePlayer.ActorNr }); // sending a draw card event to the activeplayer, including the actual data.
            RaiseEvent(101, activePlayer.Hand.Cards.Count, new int[] { _game.PlayerState.Opponent.ActorNr }); // sending a draw card event to the opponent, but without the actual data.
        }

        private Deck GetDeck(string pDeckCode, string pTurboCode) {
            Task<Deck> task = Task.Run(async () => {
                return await WebServer.GetCardsUsingCodes(pDeckCode, pTurboCode);
            });

            Deck deck = task.Result;
            if (deck != null) {
                return deck;
            }
            return null;
        }
    }
}
