using System;
using System.Collections.Generic;

namespace CardGame {
    public class Game {
        public readonly PlayerState PlayerState;
        public readonly BoardState BoardState;
        public bool Started = false;

        // Constructor
        public Game(int pMaxPlayers) {
            PlayerState = new PlayerState(pMaxPlayers);
            BoardState = new BoardState(PlayerState);
        }

        public Player AddPlayer(int pActorNr, string pUserId, Deck pDeck) {
            Player player = PlayerState.AddPlayer(pActorNr, pUserId, pDeck);
            return player;
        }

        public void RegisterFunctions(Card pCard) {
            pCard.Behaviour.RegisterFunction("SetCanAttack", this, typeof(Game).GetMethod("SetCanAttack"));
            pCard.Behaviour.RegisterFunction("SetHasShield", this, typeof(Game).GetMethod("SetHasSingleShield"));
        }

        public void SetCanAttack(MonsterCard pCard, bool pValue) {
            int value = (pValue == true) ? 1 : 0;

            PlayerState.GetPlayerById(pCard.OwnerId).BoardSide.AddOrModifyEnhancement(pCard.Id, pCard.BoardIndex, BoardSide.BoardEnhancements.Can_Attack, value, null);
        }

        public void SetHasSingleShield(MonsterCard pCard, bool pValue) {
            int value = (pValue == true) ? 1 : 0;

            PlayerState.GetPlayerById(pCard.OwnerId).BoardSide.AddOrModifyEnhancement(pCard.Id, pCard.BoardIndex, BoardSide.BoardEnhancements.Has_Single_Shield, value, null);
        }
    }
}

