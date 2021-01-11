using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace CardGame {
    [Serializable]
    public class Enhancement  {
        public IInteractable Owner;
        public readonly BoardState.BoardEnhancements BoardEnhancement;
        public readonly int Value;
        public readonly Dictionary<object, int> Properties;

        public bool Handled {
            get;
            set;
        }

        // Constructor
        public Enhancement(IInteractable pTarget, BoardState.BoardEnhancements pBoardEnhancement, int pValue) {
            Owner = pTarget;
            BoardEnhancement = pBoardEnhancement;
            Value = pValue;
            Properties = new Dictionary<object, int>();
        }

        public void AddOrModifyProperty(BoardState.BoardEnhancements pKey, int pValue) {
            if (Properties.ContainsKey(pKey)) {
                Properties[pKey] = pValue;
            } else {
                Properties.Add(pKey, pValue);
            }
        }

        public void AddOrModifyProperty(string pKey, int pValue) {
            if (Properties.ContainsKey(pKey)) {
                Properties[pKey] = pValue;
            } else {
                Properties.Add(pKey, pValue);
            }
        }

        // I like this more than returning null
        public bool GetPropertyValue(BoardState.BoardEnhancements pKey, out int pValue) {
            bool hasKey = Properties.ContainsKey(pKey);
            if (hasKey) {
                pValue = Properties[pKey];
            } else {
                pValue = 0;
            }
            return hasKey;
        }

        public byte[] Serialize() {
            using (MemoryStream ms = new MemoryStream()) {
                using (BinaryWriter bw = new BinaryWriter(ms)) {
                    bw.Write(Owner.GetId());
                    bw.Write(Owner.GetBoardIndex());
                    bw.Write(Owner.GetOwnerId());
                    bw.Write((int) BoardEnhancement);
                    bw.Write(Value);

                    return ms.ToArray();
                }
            }
        }
    }
}

