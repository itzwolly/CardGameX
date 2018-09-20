using UnityEngine;

public class HealAmountPlayer : Action {

    public override void OnEnter() {
        Debug.Log("Healing player for x");
    }

    public override void OnExit() {
        Debug.Log("Calling OnExit in: " + this);
    }

    public override void OnStay() {
        Debug.Log("Calling OnStay in: " + this);
    }
}
