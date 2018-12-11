namespace CardGame {
    using System;
    using NLua;

    public class Card : IScriptable {
        public readonly int Id;
        public readonly string Name;
        public readonly string Description;
        public int RegCost;
        public readonly int TurboCost;
        public readonly string ActionsString;
        public readonly bool IsTurbo;
        public readonly Lua Behaviour;

        public int OwnerId {
            get;
            set;
        }

        public enum CardType {
            None,
            Spell,
            Monster
        }

        public Card(int pId, string pName, string pDescription, int pRegCost, int pTurboCost, bool pIsTurbo) {
            Id = pId;
            Name = pName;
            Description = pDescription;
            RegCost = pRegCost;
            TurboCost = pTurboCost;
            IsTurbo = pIsTurbo;
            Behaviour = LuaHelper.DoFile(LuaHelper.GetScriptFolder() + pId + LuaHelper.FILE_TYPE, Game.Instance);
        }

        public static CardType ValidateType(string pCardType) {
            CardType type = (CardType) Enum.Parse(typeof(CardType), pCardType);
            if (Enum.IsDefined(typeof(CardType), pCardType)) {
                return type;
            }
            return CardType.None;
        }

        public Type GetCardType() {
            if (this is MonsterCard) {
                return typeof(MonsterCard);
            } else {
                return typeof(SpellCard);
            }
        }

        public bool IsMonster() {
            return this is MonsterCard;
        }

        public bool IsSpell() {
            return this is SpellCard;
        }

        public Lua GetBehaviour() {
            return Behaviour;
        }

        public object[] CallFunction(string pFunction, params object[] pArgs) {
            LuaFunction function = Behaviour.GetFunction(pFunction);

            if (function != null) {
                return function.Call(pArgs);
            } else {
                if (IsMonster()) {
                    function = Game.Instance.MonsterDefaultBehaviour.GetFunction(LuaHelper.MONSTER_DEFAULT_NAMESPACE + pFunction);
                } else {
                    function = Game.Instance.SpellDefaultBehaviour.GetFunction(LuaHelper.SPELL_DEFAULT_NAMESPACE + pFunction);
                }
                return function.Call(pArgs);
            }
        }
    }
}