using System;

public abstract class Action  {
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnStay();

    public static Action CreateInstance(string pFullyQualifiedName) {
        Type t = Type.GetType(pFullyQualifiedName);
        return (Action) Activator.CreateInstance(t);
    }
}
