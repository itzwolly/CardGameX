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
using NLua;
using System.Linq;

namespace CardGamePlugins {

    public class CardGameBehaviour : PluginBase {
        public const string NAME = "CardGameBehaviour";
        private const int TURN_TIME_IN_MS = 20000; // 60000

        public override string Name {
            get { return NAME; }
        }

        public override void OnCreateGame(ICreateGameCallInfo info) {
            string deckCode = info.OperationRequest.Parameters[189] as string;
            string turboCode = info.OperationRequest.Parameters[190] as string;
            Deck deck = GetDeck(deckCode, turboCode);

            if (deck == null) {
                info.Fail("No (valid) deck can be found on the server. If you haven't made one, don't forget to do so first.");
                return;
            }
            
            Game.Instance.AddPlayer(1, info.UserId, deck);

            base.OnCreateGame(info);
        }

        public override void OnJoin(IJoinGameCallInfo info) {
            string deckCode = info.OperationRequest.Parameters[189] as string;
            string turboCode = info.OperationRequest.Parameters[190] as string;
            Deck deck = GetDeck(deckCode, turboCode);

            if (deck == null) {
                info.Fail("No (valid) deck can be found on the server. If you haven't made one, don't forget to do so first.");
                return;
            }

            Game.Instance.AddPlayer(info.ActorNr, info.UserId, deck);

            base.OnJoin(info);

            if (info.IsSucceeded) {
                InitiateFullRoomSequence();
            }
        }

        public override void OnRaiseEvent(IRaiseEventCallInfo info) {
            if (info.Request.EvCode == 117) { // ATTACK
                Hashtable data = (Hashtable) info.Request.Data;
                PlayerState playerState = Game.Instance.PlayerState;
                BoardState boardState = Game.Instance.BoardState;

                int targetId = (int) data["targetid"];
                int targetOwnerId = 0;
                int targetIndex = 0;

                if (!playerState.IsPlayerId(targetId)) {
                    targetOwnerId = (int) data["targetownerid"];
                    targetIndex = (int) data["targetindex"];
                }

                int attackerId = (int) data["attackerid"];
                int attackerOwnerId = (int) data["attackerownerid"];
                int attackerIndex = (int) data["attackerindex"];

                Player activePlayer = playerState.GetActivePlayer();
                Player opponent = playerState.Opponent;

                IScriptable sAttacker = (playerState.IsPlayerId(attackerId)) ? activePlayer as IScriptable : activePlayer.BoardSide.GetSlotByIndex(attackerIndex).Monster;
                IScriptable sTarget = (playerState.IsPlayerId(targetId)) ? opponent as IScriptable : opponent.BoardSide.GetSlotByIndex(targetIndex).Monster;

                if (sAttacker == null || sTarget == null) {
                    info.Cancel();
                    return;
                }

                IInteractable iAttacker = (sAttacker as IInteractable);
                IInteractable iTarget = (sTarget as IInteractable);

                object[] beforeAttackResult = sAttacker.CallFunction("BeforeAttack", iAttacker, iTarget);
                object[] onAttackResult = sAttacker.CallFunction("OnAttack", iAttacker, iTarget);

                if (onAttackResult[0] is bool) {
                    bool attackResult = (bool) onAttackResult[0];
                    if (attackResult) {
                        object[] afterAttackResult = sAttacker.CallFunction("AfterAttack", iAttacker, iTarget);

                        if (iAttacker.GetHealth() <= 0) {
                            sAttacker.CallFunction("OnDeath", iAttacker);
                        }

                        if (iTarget.GetHealth() <= 0) {
                            sTarget.CallFunction("OnDeath", iTarget);
                        }

                        //Hashtable toSend = new Hashtable();
                        //List<Enhancement> enhancements = boardState.GetEnhancements().Where(o => o.Handled == false).ToList();
                        //if (enhancements.Count > 0) {
                        //    toSend.Add("amount", enhancements.Count);

                        //    for (int i = 0; i < enhancements.Count; i++) {
                        //        Enhancement enh = enhancements[i];
                        //        toSend.Add("enhancement-" + i, enh.Serialize());
                        //        enh.Handled = true;
                        //    }

                        //    RaiseEvent(118, toSend, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                        //}

                        //if (iAttacker.GetHealth() <= 0) {
                        //    if (iAttacker is MonsterCard) {
                        //        activePlayer.BoardSide.KillMonster(iAttacker as MonsterCard);
                        //    }
                        //}
                        //if (iTarget.GetHealth() <= 0) {
                        //    if (iTarget is MonsterCard) {
                        //        opponent.BoardSide.KillMonster(iTarget as MonsterCard);
                        //    }
                        //}

                        Hashtable toSend;
                        if (Game.Instance.BoardState.GetUnhandledEnhancements(out toSend)) {
                            RaiseEvent(118, toSend, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                        }
                    }
                }

                info.Cancel();
                return;
            } else if (info.Request.EvCode == PlaySpellCardEvent.EVENT_CODE) {
                BoardState boardState = Game.Instance.BoardState;
                PlayerState playerState = Game.Instance.PlayerState;
                PlaySpellCardEvent playSCEvent = new PlaySpellCardEvent(playerState);

                List<EventResponse> responses;
                bool handled = playSCEvent.Handle(info, out responses);
                if (handled) {
                    Player activePlayer = playerState.GetActivePlayer();

                    // raise card played event.. //
                    Hashtable data = (Hashtable) info.Request.Data;
                    data.Add("mana", activePlayer.CurrentMana);

                    int cardId = (int) data["cardid"];
                    int index = (int) data["cardindex"];

                    RaiseEvent(106, data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    // end raise card played event //

                    //Hashtable toSend = new Hashtable();
                    //List<Enhancement> enhancements = boardState.GetEnhancements().Where(o => o.Handled == false).ToList();
                    //if (enhancements.Count > 0) {
                    //    toSend.Add("amount", enhancements.Count);

                    //    for (int i = 0; i < enhancements.Count; i++) {
                    //        Enhancement enh = enhancements[i];
                    //        toSend.Add("enhancement-" + i, enh.Serialize());
                    //        enh.Handled = true;
                    //    }

                    //    RaiseEvent(118, toSend, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    //}

                    Hashtable toSend;
                    if (Game.Instance.BoardState.GetUnhandledEnhancements(out toSend)) {
                        RaiseEvent(118, toSend, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    }
                }

                info.Cancel();
                return;
            } else if (info.Request.EvCode == PlayMonsterCardEvent.EVENT_CODE) {
                BoardState boardState = Game.Instance.BoardState;
                PlayerState playerState = Game.Instance.PlayerState;
                PlayMonsterCardEvent playMCEvent = new PlayMonsterCardEvent(playerState, boardState);

                List<EventResponse> responses;
                bool handled = playMCEvent.Handle(info, out responses);
                if (handled) {
                    Player activePlayer = playerState.GetActivePlayer();

                    // raise card played event.. //
                    Hashtable data = (Hashtable) info.Request.Data;
                    data.Add("mana", activePlayer.CurrentMana);
                    data.Add("totalmana", activePlayer.TotalMana);

                    int cardId = (int) data["cardid"];
                    int index = (int) data["cardindex"];
                    int boardIndex = (int) data["boardindex"];

                    RaiseEvent(106, data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    // end raise card played event //

                    foreach (EventResponse response in responses) {
                        RaiseEvent(response.EventCode, response.Data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    }

                    //Hashtable toSend = new Hashtable();
                    //List<Enhancement> enhancements = boardState.GetEnhancements().Where(o => o.Handled == false).ToList();
                    //if (enhancements.Count > 0) {
                    //    toSend.Add("amount", enhancements.Count);

                    //    for (int i = 0; i < enhancements.Count; i++) {
                    //        Enhancement enh = enhancements[i];
                    //        toSend.Add("enhancement-" + i, enh.Serialize());
                    //        enh.Handled = true;
                    //    }

                    //    RaiseEvent(118, toSend, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    //}

                    Hashtable toSend;
                    if (Game.Instance.BoardState.GetUnhandledEnhancements(out toSend)) {
                        RaiseEvent(118, toSend, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                    }
                }

                info.Cancel();
                return;
            } else if (info.Request.EvCode == 98) { // LoadedGameplayScene.EVENT_CODE a.k.a very start of the game
                if (!Game.Instance.Started) {
                    PluginHost.CreateTimer(SendStartTurnEvent, 200, TURN_TIME_IN_MS);
                    PluginHost.CreateTimer(DrawCard, 300, TURN_TIME_IN_MS + 300); // small delay.. as to not spam the client with events

                    Game.Instance.Started = true;

                    info.Cancel();
                    return;
                }
            } else if (info.Request.EvCode == RequestTurboCardsEvent.EVENT_CODE) {
                PlayerState state = Game.Instance.PlayerState;
                RequestTurboCardsEvent requestTurboEvent = new RequestTurboCardsEvent(state);

                List<EventResponse> responses;
                bool handled = requestTurboEvent.Handle(info, out responses);
                if (handled) {
                    foreach (EventResponse response in responses) {
                        RaiseEvent(response.EventCode, response.Data, response.Receivers);
                    }
                }

                info.Cancel();
                return;
            } else if (info.Request.EvCode == RequestCloseTurboWindowEvent.EVENT_CODE) {
                PlayerState state = Game.Instance.PlayerState;
                RequestCloseTurboWindowEvent requestTurboCloseEvent = new RequestCloseTurboWindowEvent(state);

                List<EventResponse> responses;
                bool handled = requestTurboCloseEvent.Handle(info, out responses);
                if (handled) {
                    foreach (EventResponse response in responses) {
                        RaiseEvent(response.EventCode, response.Data, response.Receivers);
                    }
                }

                info.Cancel();
                return;
            } else if (info.Request.EvCode == AddTurboCardEvent.EVENT_CODE) {
                PlayerState state = Game.Instance.PlayerState;
                AddTurboCardEvent addTurboCardEvent = new AddTurboCardEvent(state);

                List<EventResponse> responses;
                bool handled = addTurboCardEvent.Handle(info, out responses);
                if (handled) {
                    Player activePlayer = state.GetActivePlayer();
                    Hashtable data = (Hashtable) info.Request.Data;
                    
                    int cardId = (int) data["cardid"];
                    int cardIndex = (int) data["cardindex"];

                    Card card = activePlayer.Deck.GetTurboCard(cardId);

                    if (card != null && activePlayer.CurrentTurbo >= card.TurboCost) {
                        Hashtable fullTableData = new Hashtable();
                        fullTableData.Add("cardid", card.Id);
                        fullTableData.Add("name", card.Name);
                        fullTableData.Add("type", card.GetCardType().Name);
                        fullTableData.Add("turbocost", card.TurboCost);
                        fullTableData.Add("description", card.Description);

                        if (card.IsMonster()) {
                            MonsterCard monster = card as MonsterCard;
                            fullTableData.Add("attack", monster.Attack);
                            fullTableData.Add("health", monster.Health);
                        }

                        card.RegCost = 0; // change regular cost of turbo cards to 0
                        activePlayer.CurrentTurbo -= card.TurboCost;

                        activePlayer.Hand.AddCard(card);
                        activePlayer.Deck.RemoveTurboCard(card);

                        fullTableData.Add("regcost", card.RegCost);
                        fullTableData.Add("handsize", activePlayer.Hand.Cards.Count);
                        
                        Hashtable partialTableData = new Hashtable();
                        partialTableData.Add("handsize", activePlayer.Hand.Cards.Count);
                        partialTableData.Add("cardindex", cardIndex);
                        partialTableData.Add("turbo", activePlayer.CurrentTurbo);

                        RaiseEvent(115, partialTableData, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
                        RaiseEvent(116, fullTableData, new int[] { activePlayer.ActorNr }); // sending a draw card event to the activeplayer, including the actual data.
                    }
                }
                info.Cancel();
                return;
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
            CloseTurboWindow();

            // TODO: Before starting new turn, check if there are any events waiting to be executed.
            Player activePlayer = Game.Instance.PlayerState.SetActivePlayer(); 
            activePlayer.TotalMana++;
            activePlayer.TotalTurbo++;

            Hashtable data = new Hashtable();
            data.Add("userid", activePlayer.UserId);
            data.Add("mana", 1);
            data.Add("turbo", 1);
            data.Add("turntimelimit", TURN_TIME_IN_MS / 1000);

            RaiseEvent(100, data, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);

            foreach (MonsterCard card in activePlayer.BoardSide.GetMonsters()) {
                IScriptable scriptable = card as IScriptable;
                scriptable.CallFunction("OnStartTurn", card);
            }

            //Hashtable toSend = new Hashtable();
            //List<Enhancement> enhancements = Game.Instance.BoardState.GetEnhancements().Where(o => o.Handled == false).ToList();
            //if (enhancements.Count > 0) {
            //    toSend.Add("amount", enhancements.Count);

            //    for (int i = 0; i < enhancements.Count; i++) {
            //        Enhancement enh = enhancements[i];
            //        toSend.Add("enhancement-" + i, enh.Serialize());
            //        enh.Handled = true;
            //    }

            //    //RaiseEvent(118, toSend, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
            //}

            Hashtable toSend;
            if (Game.Instance.BoardState.GetUnhandledEnhancements(out toSend)) {
                RaiseEvent(118, toSend, ReciverGroup.All, activePlayer.ActorNr, (byte) CacheOperation.AddToRoomCache);
            }
        }

        private void DrawCard() {
            Player activePlayer = Game.Instance.PlayerState.GetActivePlayer();
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

            activePlayer.Hand.AddCard(drawn);
            data.Add("handsize", activePlayer.Hand.Cards.Count);

            RaiseEvent(101, data, new int[] { activePlayer.ActorNr }); // sending a draw card event to the activeplayer, including the actual data.
            RaiseEvent(101, activePlayer.Hand.Cards.Count, new int[] { Game.Instance.PlayerState.Opponent.ActorNr }); // sending a draw card event to the opponent, but without the actual data.
        }

        private void CloseTurboWindow() {
            PlayerState state = Game.Instance.PlayerState;
            
            if (state.Data.ContainsKey((byte) PlayerState.PlayerStateKeys.TURBO_VIEWING_KEY)) {
                int ownerNr = state.Data[(byte) PlayerState.PlayerStateKeys.TURBO_VIEWING_KEY];

                RaiseEvent(113, null, ReciverGroup.All, ownerNr, (byte) CacheOperation.AddToRoomCache);

                state.Data.Remove((byte) PlayerState.PlayerStateKeys.TURBO_VIEWING_KEY);
            }
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
