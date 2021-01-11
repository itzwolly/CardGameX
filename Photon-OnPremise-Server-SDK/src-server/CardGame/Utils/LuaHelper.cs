using NLua;
using System;
using System.Collections.Generic;
using System.IO;

namespace CardGame {
    public class LuaHelper {
        public const string FILE_TYPE = ".lua";
        public const string MONSTER_DEFAULT_SCRIPT = "monster_default";
        public const string SPELL_DEFAULT_SCRIPT = "spell_default";
        public const string PLAYER_SCRIPT = "player";
        public const string MONSTER_DEFAULT_NAMESPACE = "Monster.";
        public const string SPELL_DEFAULT_NAMESPACE = "Spell.";

        //public static string FileType {
        //    get { return ".lua"; }
        //}
        //public static string MonsterDefaultScript {
        //    get { return "monster_default.lua"; }
        //}
        //public static string SpellDefaultScript {
        //    get { return "spell_default.lua"; }
        //}
        //public static string PlayerScript {
        //    get { return "player.lua"; }
        //}
        //public static string MonsterDefaultNamespace {
        //    get { return "Monster."; }
        //}
        //public static string SpellDefaultNamespace {
        //    get { return "Spell."; }
        //}

        public static string GetScriptFolder() {
            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/CardGame/Scripts/";
        }

        public static Lua DoFile(string pFileName, Game pGame) {
            Lua behaviour = new Lua();
            //behaviour.LoadCLRPackage();
            behaviour.DoFile(pFileName);
            pGame.RegisterFunctions(behaviour);
            return behaviour;
        }
    }
}

