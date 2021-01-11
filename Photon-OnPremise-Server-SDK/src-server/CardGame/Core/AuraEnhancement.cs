using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
    public class AuraEnhancement {
        public readonly IInteractable Owner;
        public readonly BoardState.BoardEnhancements BoardEnhancement;
        public readonly TargetCollection TargetCollection;
        public readonly int Value;

        private List<Enhancement> _enhancements;

        public AuraEnhancement(IInteractable pOwner, BoardState.BoardEnhancements pEnhancement, TargetCollection.TargetType pTargetType, int pValue) {
            Owner = pOwner;
            BoardEnhancement = pEnhancement;
            TargetCollection = new TargetCollection(pOwner, pTargetType);
            Value = pValue;
            _enhancements = new List<Enhancement>();
        }

        public bool ShouldReceiveEffects(IInteractable pTarget) {
            return TargetCollection.GetTargets().Contains(pTarget);
        }

        public List<Enhancement> GetEnhancements() {
            return _enhancements;
        }
    }
}

