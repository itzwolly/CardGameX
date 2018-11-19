using System;

namespace CardGame {
    [Serializable]
    public class CardInfo {
        public int id;
        public string name;
        public string description;
        public string type;
        public int? attack;
        public int? health;
        public int regcost;
        public int turbocost;
        public string actions;
    }
}

