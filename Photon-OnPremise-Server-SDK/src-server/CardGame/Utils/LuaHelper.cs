using System;
using System.Collections.Generic;
using System.IO;

public class LuaHelper {
    public static string FileType {
        get { return ".lua"; }
    }

    public static string GetScriptFolder() {
        return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\CardGame\\Scripts\\";
    }
}
