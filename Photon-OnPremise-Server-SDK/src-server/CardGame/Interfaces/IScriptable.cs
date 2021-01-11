namespace CardGame {
    using NLua;

    public interface IScriptable {
        Lua GetBehaviour();
        object[] CallFunction(string pFunction, params object[] pArgs);
    }
}

