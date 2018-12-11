using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame {
    public class BoardSlot {
        public readonly int BoardIndex;
        public readonly Player Owner;

        public MonsterCard Monster {
            get { return _monster; }
        }

        private MonsterCard _monster;

        // Constructor
        public BoardSlot(int pIndex, MonsterCard pMonster, Player pOwner) {
            BoardIndex = pIndex;
            _monster = pMonster;
            Owner = pOwner;
        }

        public void UpdateMonster(MonsterCard pMonster) {
            if (pMonster == null) { return; }
            _monster = pMonster;
        }

        public void ClearMonster() {
            _monster = null;
        }
    }
}

