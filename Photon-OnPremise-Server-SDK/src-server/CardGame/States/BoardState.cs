using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace CardGame {
    public class BoardState {
        public const int BOARD_SIDE_SIZE = 5; // Amount of slots each side of the board should have..

        private List<Enhancement> _enhancements;
        private List<AuraEnhancement> _auraEnhancement;

        public BoardState() {
            _enhancements = new List<Enhancement>();
            _auraEnhancement = new List<AuraEnhancement>();
        }

        public Enhancement AddEnhancement(IInteractable pOwner, BoardEnhancements pBoardEnhancement, int pValue) {
            Enhancement enhancement = new Enhancement(pOwner, pBoardEnhancement, pValue);
            _enhancements.Add(enhancement);
            ExecuteAdd(enhancement);

            return enhancement;
        }

        public AuraEnhancement AddAuraEnhancement(IInteractable pOwner, BoardEnhancements pEnhancement, TargetCollection.TargetType pTargetType, int pValue) {
            AuraEnhancement aura = new AuraEnhancement(pOwner, pEnhancement, pTargetType, pValue);
            _auraEnhancement.Add(aura);

            for (int i = 0; i < aura.TargetCollection.GetTargets().Count; i++) {
                IInteractable target = aura.TargetCollection.GetTargets()[i];
                Enhancement enh = AddEnhancement(target, aura.BoardEnhancement, aura.Value);
                aura.GetEnhancements().Add(enh);
            }
            return aura;
        }

        public void ValidateAuras(IInteractable pTarget) {
            for (int i = 0; i < _auraEnhancement.Count; i++) {
                AuraEnhancement aura = _auraEnhancement[i];
                if (aura.ShouldReceiveEffects(pTarget)) {
                    Enhancement enh = Game.Instance.BoardState.AddEnhancement(pTarget, aura.BoardEnhancement, aura.Value);
                    aura.GetEnhancements().Add(enh);
                }
            }
        }

        public void CleanAuras(IInteractable pTarget) {
            for (int i = _auraEnhancement.Count - 1; i >= 0; i--) {
                if (_auraEnhancement[i].Owner != pTarget) { continue; }

                AuraEnhancement aura = _auraEnhancement[i];
                for (int j = aura.GetEnhancements().Count - 1; j >= 0; j--) {
                    Enhancement enh = aura.GetEnhancements()[j];
                    NegateEnhancement(enh);
                    aura.GetEnhancements().Remove(enh);
                }
                _auraEnhancement.Remove(aura);
            }
        }

        public object GetNewestEnhancementValue(IInteractable pOwner, BoardEnhancements pBoardEnhancement) {
            Enhancement enh = GetNewestEnhancement(pOwner, pBoardEnhancement);
            if (enh != null) {
                return enh.Value;
            } else {
                return null;
            }
        }

        public Enhancement GetNewestEnhancement(IInteractable pOwner, BoardEnhancements pBoardEnhancement) {
            Enhancement enh = _enhancements.LastOrDefault(o => o.Owner == pOwner && o.BoardEnhancement == pBoardEnhancement);
            return enh;
        }

        public List<AuraEnhancement> GetAuraEnhancements() {
            return _auraEnhancement;
        }

        public List<Enhancement> GetEnhancements() {
            return _enhancements;
        }

        public bool GetUnhandledEnhancements(out Hashtable pTable) {
            List<Enhancement> enhancements = Game.Instance.BoardState.GetEnhancements().Where(o => o.Handled == false).ToList();
            Hashtable table = new Hashtable();
            if (enhancements.Count > 0) {
                table.Add("amount", enhancements.Count);
                for (int i = 0; i < enhancements.Count; i++) {
                    Enhancement enh = enhancements[i];
                    table.Add("enhancement-" + i, enh.Serialize());

                    if (enh.Owner.GetHealth() <= 0) {
                        if (enh.Owner is MonsterCard) { // Its a monster..
                            Game.Instance.PlayerState.GetPlayerById(enh.Owner.GetOwnerId()).BoardSide.KillMonster(enh.Owner as MonsterCard); // So kill it..
                        } else { // Is Player
                            // TODO: Do something here..
                        }
                    }
                    enh.Handled = true;
                }
                pTable = table;
                return true;
            }
            pTable = null;
            return false;            
        }

        public List<Enhancement> GetEnhancements(IInteractable pOwner) {
            return _enhancements.Where(o => o.Owner == pOwner).ToList();
        }

        public int GetEnhancementCount(IInteractable pOwner, BoardEnhancements pBoardEnhancement) {
            if (pOwner == null) { return 0; }
            return _enhancements.Count(o => o.Owner == pOwner && o.BoardEnhancement == pBoardEnhancement);
        }

        public int GetEnhancementCount() {
            return _enhancements.Count;
        }

        public void RemoveAllEnhancements(IInteractable pOwner) {
            if (pOwner == null) { return; }

            List<Enhancement> enh = GetEnhancements(pOwner);
            for (int i = enh.Count - 1; i >= 0; i--) {
                _enhancements.Remove(enh[i]);
            }
        }

        public static BoardEnhancements Serialize(string pEnhancement) {
            BoardEnhancements enhancement;
            bool parsed = Enum.TryParse(pEnhancement, out enhancement);
            if (parsed) {
                return enhancement;
            }
            return BoardEnhancements.None;
        }

        private void ExecuteAdd(Enhancement pEnhancement) {
            BoardEnhancements boardEnhancement = pEnhancement.BoardEnhancement;
            switch (boardEnhancement) {
                case BoardEnhancements.Set_Health: {
                        break;
                    }
                case BoardEnhancements.Set_Attack: {
                        break;
                    }
                case BoardEnhancements.Add_Health: {
                        IInteractable owner = pEnhancement.Owner;
                        int healthToAdd = pEnhancement.Value;

                        pEnhancement.AddOrModifyProperty(boardEnhancement, healthToAdd);

                        owner.SetHealth((int) owner.GetHealth() + healthToAdd);
                        break;
                    }
                case BoardEnhancements.Add_Attack: {
                        IInteractable owner = pEnhancement.Owner;
                        int attackToAdd = pEnhancement.Value;

                        pEnhancement.AddOrModifyProperty(boardEnhancement, attackToAdd);

                        owner.SetAttack((int) owner.GetAttack() + attackToAdd);
                        break;
                    }
                case BoardEnhancements.Can_Attack:
                case BoardEnhancements.Has_Single_Shield:
                case BoardEnhancements.Spell_Taunt:
                case BoardEnhancements.None:
                default:
                    break;
            }
        }

        private void NegateEnhancement(Enhancement pEnhancement) {
            BoardEnhancements boardEnhancement = pEnhancement.BoardEnhancement;
            switch (boardEnhancement) {
                case BoardEnhancements.Set_Health: // TODO: ADD
                case BoardEnhancements.Set_Attack: // TODO: ADD
                case BoardEnhancements.Add_Health: {
                        IInteractable owner = pEnhancement.Owner;

                        int healthToAdd;
                        if (pEnhancement.GetPropertyValue(boardEnhancement, out healthToAdd)) {
                            AddEnhancement(owner, boardEnhancement, -healthToAdd);
                        }
                        break;
                    }
                case BoardEnhancements.Add_Attack: {
                        IInteractable owner = pEnhancement.Owner;

                        int attackToAdd;
                        if (pEnhancement.GetPropertyValue(boardEnhancement, out attackToAdd)) {
                            AddEnhancement(owner, boardEnhancement, -attackToAdd);
                        }
                        break;
                    }
                case BoardEnhancements.Can_Attack:
                case BoardEnhancements.Has_Single_Shield:
                case BoardEnhancements.Spell_Taunt:
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
            // Lure
            Spell_Taunt,
        }
    }
}
